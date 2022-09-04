// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using Rev.AuthPermissions.BaseCode.CommonCode;
using Rev.AuthPermissions.BaseCode.DataLayer.Classes.SupportTypes;
using StatusGeneric;
using System.ComponentModel.DataAnnotations;

namespace Rev.AuthPermissions.BaseCode.DataLayer.Classes
{
	/// <summary>
	/// This is used for multi-tenant systems
	/// </summary>
	public class Tenant : INameToShowOnException
	{
		private HashSet<RoleToPermissions> _tenantRoles;

		private Tenant()
		{ } //Needed by EF Core

		private Tenant(string tenantId, string tenantName)
		{
			TenantId = tenantId;
			TenantName = tenantName?.Trim() ?? throw new ArgumentNullException(nameof(tenantName));
		}

		/// <summary>
		/// This defines a tenant in a single tenant multi-tenant system.
		/// </summary>
		/// <param name="tenantName"></param>
		/// <param name="tenantRoles">Optional: add Roles that have a <see cref="RoleTypes"/> of
		/// <see cref="RoleTypes.TenantAutoAdd"/> or <see cref="RoleTypes.TenantAdminAdd"/></param>
		public static IStatusGeneric<Tenant> CreateSingleTenant(string tenantId, string tenantName, List<RoleToPermissions> tenantRoles = null)
		{
			var newInstance = new Tenant(tenantId, tenantName);
			var status = CheckRolesAreAllTenantRolesAndSetTenantRoles(tenantRoles, newInstance);
			return status;
		}

		/// <summary>
		/// Tenant primary key
		/// </summary>
		public string TenantId { get; private set; }

		/// <summary>
		/// This is the name defined for this tenant, and must be unique.
		/// </summary>
		[Required(AllowEmptyStrings = false)]
		[MaxLength(AuthDbConstants.TenantFullNameSize)]
		public string TenantName { get; private set; }

		/// <summary>
		/// This is true if the tenant has its own database.
		/// This is used by single-level tenants to return true for the query filter
		/// Also provides a quick way to find out what databases are used and how many tenants are in each database
		/// </summary>
		public bool HasOwnDb { get; private set; }

		/// <summary>
		/// If sharding is turned on then this will contain the name of database data
		/// in the shardingsettings.json file. This must not be null.
		/// </summary>
		public string DatabaseInfoName { get; private set; }

		/// <summary>
		/// This holds any Roles that have been specifically
		/// </summary>
		public IReadOnlyCollection<RoleToPermissions> TenantRoles => _tenantRoles?.ToList();

		/// <summary>
		/// Easy way to see the tenant and its key
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return $"{TenantName}: Key = {this.GetTenantDataKey()}";
		}

		//--------------------------------------------------
		// Exception Error name

		/// <summary>
		/// Used when there is an exception
		/// </summary>
		public string NameToUseForError => TenantName;

		//----------------------------------------------------
		//access methods

		/// <summary>
		/// This allows you to change the sharding information for this tenant
		/// </summary>
		/// <param name="nawDatabaseInfoName">This contains the name of database data in the shardingsettings.json file</param>
		/// <param name="hasOwnDb">true if it is the only tenant in its database</param>
		public void UpdateShardingState(string nawDatabaseInfoName, bool hasOwnDb)
		{
			DatabaseInfoName = nawDatabaseInfoName ?? throw new ArgumentNullException(nameof(nawDatabaseInfoName));
			HasOwnDb = hasOwnDb;
		}

		/// <summary>
		/// This will provide a single tenant name.
		/// If its an hierarchical tenant, then it will be the last name in the hierarchy
		/// </summary>
		/// <returns></returns>
		public string GetTenantName() => ExtractEndLeftTenantName(TenantName);

		/// <summary>
		/// This is the official way to combine the parent name and the individual tenant name
		/// </summary>
		/// <param name="thisTenantName">name for this specific tenant level</param>
		/// <param name="fullParentName"></param>
		/// <returns></returns>
		public static string CombineParentNameWithTenantName(string thisTenantName, string fullParentName)
		{
			if (thisTenantName == null) throw new ArgumentNullException(nameof(thisTenantName));
			return fullParentName == null ? thisTenantName : $"{fullParentName} | {thisTenantName}";
		}

		/// <summary>
		/// This updates the tenant name
		/// </summary>
		/// <param name="newNameAtThisLevel"></param>
		public void UpdateTenantName(string newNameAtThisLevel)
		{
			if (newNameAtThisLevel == null) throw new ArgumentNullException(nameof(newNameAtThisLevel));
			TenantName = newNameAtThisLevel.Trim();
		}

		/// <summary>
		/// This will replace the current tenant roles with a new set of tenant roles
		/// </summary>
		/// <param name="tenantRoles"></param>
		/// <exception cref="AuthPermissionsException"></exception>
		/// <exception cref="AuthPermissionsBadDataException"></exception>
		public IStatusGeneric UpdateTenantRoles(List<RoleToPermissions> tenantRoles)
		{
			if (_tenantRoles == null)
				throw new AuthPermissionsException(
					$"You must include the tenant's {nameof(TenantRoles)} in your query before you can add/remove an tenant role.");

			var status = new StatusGenericHandler();
			return status.CombineStatuses(CheckRolesAreAllTenantRolesAndSetTenantRoles(tenantRoles, this));
		}

		/// <summary>
		/// This will return a single tenant name. If it's hierarchical it returns the final name
		/// </summary>
		/// <param name="fullTenantName"></param>
		/// <returns></returns>
		public static string ExtractEndLeftTenantName(string fullTenantName)
		{
			var lastIndex = fullTenantName.LastIndexOf('|');
			var thisLevelTenantName = lastIndex < 0 ? fullTenantName : fullTenantName.Substring(lastIndex + 1).Trim();
			return thisLevelTenantName;
		}

		//-------------------------------------------------------
		// private methods

		/// <summary>
		/// This checks that the given roles have a <see cref="RoleToPermissions.RoleType"/> that can be added to a tenant.
		/// If no errors (and roles aren't null) the <see cref="_tenantRoles"/> collection is updated, otherwise the status is returned with errors
		/// </summary>
		/// <param name="tenantRoles">The list of roles to added/updated to <see param="thisTenant"/> instance. Can be null</param>
		/// <param name="thisTenant">the current instance of the tenant</param>
		/// <exception cref="AuthPermissionsBadDataException"></exception>
		/// <returns>status, with the <see param="thisTenant"/> instance if no errors.</returns>
		private static IStatusGeneric<Tenant> CheckRolesAreAllTenantRolesAndSetTenantRoles(List<RoleToPermissions> tenantRoles, Tenant thisTenant)
		{
			var status = new StatusGenericHandler<Tenant>();
			status.SetResult(thisTenant);

			var badRoles = tenantRoles?
				.Where(x => x.RoleType != RoleTypes.TenantAutoAdd && x.RoleType != RoleTypes.TenantAdminAdd)
				.ToList() ?? new List<RoleToPermissions>();

			foreach (var badRole in badRoles)
			{
				status.AddError(
					$"The Role '{badRole.RoleName}' is not a tenant role, i.e. only roles with a {nameof(RoleToPermissions.RoleType)} of " +
					$"{nameof(RoleTypes.TenantAutoAdd)} or {nameof(RoleTypes.TenantAdminAdd)} can be added to a tenant.");
			}

			if (status.HasErrors || tenantRoles == null)
				return status;

			thisTenant._tenantRoles = new HashSet<RoleToPermissions>(tenantRoles);
			return status;
		}
	}
}