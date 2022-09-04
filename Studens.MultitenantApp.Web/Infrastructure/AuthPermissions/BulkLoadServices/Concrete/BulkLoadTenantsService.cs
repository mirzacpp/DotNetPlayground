// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;
using Rev.AuthPermissions.AdminCode;
using Rev.AuthPermissions.BaseCode;
using Rev.AuthPermissions.BaseCode.DataLayer.Classes;
using Rev.AuthPermissions.BaseCode.DataLayer.Classes.SupportTypes;
using Rev.AuthPermissions.BaseCode.SetupCode;
using StatusGeneric;
using Studens.MultitenantApp.Web.Data;

namespace Rev.AuthPermissions.BulkLoadServices.Concrete
{
	/// <summary>
	/// Bulk load multiple tenants from a list of <see cref="BulkLoadTenantDto"/>
	/// This works with a single-level tenant scheme and a hierarchical tenant scheme
	/// </summary>
	[Obsolete("Remove this approach and use data seed contributors.")]
	public class BulkLoadTenantsService : IBulkLoadTenantsService
	{
		private readonly ApplicationDbContext _context;
		private Lazy<List<RoleToPermissions>> _lazyRoles;

		/// <summary>
		/// requires access to the AuthPermissionsDbContext
		/// </summary>
		/// <param name="context"></param>
		public BulkLoadTenantsService(ApplicationDbContext context)
		{
			_context = context;
			_lazyRoles = new Lazy<List<RoleToPermissions>>(() =>
				_context.RoleToPermissions.Where(x =>
						x.RoleType == RoleTypes.TenantAutoAdd || x.RoleType == RoleTypes.TenantAdminAdd)
					.ToList());
		}

		/// <summary>
		/// This allows you to add tenants to the database on startup.
		/// It gets the definition of each tenant from the <see cref="BulkLoadTenantDto"/> class
		/// </summary>
		/// <param name="tenantSetupData">If you are using a single layer then each line contains the a tenant name
		/// </param>
		/// <param name="options">The AuthPermissionsOptions to check what type of tenant setting you have</param>
		/// <returns></returns>
		public async Task<IStatusGeneric> AddTenantsToDatabaseAsync(List<BulkLoadTenantDto> tenantSetupData, AuthPermissionsOptions options)
		{
			var status = new StatusGenericHandler();

			if (tenantSetupData == null || !tenantSetupData.Any())
				return status;

			//Check the options are set
			if (!options.TenantType.IsMultiTenant())
				return status.AddError(
					$"You must set the options {nameof(AuthPermissionsOptions.TenantType)} to allow tenants to be processed");

			//This takes a COPY of the data because the code stores a tracked tenant in the database
			var tenantsSetupCopy = tenantSetupData.ToList();

			if (options.TenantType.IsSingleLevel())
			{
				var duplicateNames = tenantsSetupCopy.Select(x => x.TenantName)
					.GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
				duplicateNames.ForEach(x => status.AddError($"There is already a Tenant with the name '{x}'"));

				if (status.HasErrors)
					return status;

				foreach (var tenantDefinition in tenantsSetupCopy)
				{
					var rolesStatus = GetCheckTenantRoles(tenantDefinition.TenantRolesCommaDelimited,
						tenantDefinition.TenantName);
					status.CombineStatuses(rolesStatus);
					var tenantStatus = Tenant.CreateSingleTenant(Guid.NewGuid().ToString(), tenantDefinition.TenantName, rolesStatus.Result);

					if (status.CombineStatuses(tenantStatus).IsValid)
					{
						if ((options.TenantType & TenantTypes.AddSharding) != 0)
							tenantStatus.Result.UpdateShardingState(options.ShardingDefaultDatabaseInfoName, false);
						_context.Add(tenantStatus.Result);
					}
				}

				if (status.HasErrors)
					return status;

				return await _context.SaveChangesWithChecksAsync();
			}

			return status;
		}

		//-----------------------------------------------------------
		//private parts

		private IStatusGeneric<List<RoleToPermissions>> GetCheckTenantRoles(string tenantRolesCommaDelimited, string fullTenantName)
		{
			var status = new StatusGenericHandler<List<RoleToPermissions>>();

			if (tenantRolesCommaDelimited == null)
				return status.SetResult(null);

			var roleNames = tenantRolesCommaDelimited.Split(',').Select(x => x.Trim())
				.Distinct().ToList();

			//check provided role names are in the database
			var notFoundNames = roleNames
				.Where(x => !_lazyRoles.Value.Select(y => y.RoleName).Contains(x)).ToList();

			foreach (var notFoundName in notFoundNames)
			{
				status.AddError($"Tenant '{fullTenantName}': the role called '{notFoundName}' was not found. Either it is misspent or " +
								$"the {nameof(RoleToPermissions.RoleType)} must be {nameof(RoleTypes.TenantAutoAdd)} or {nameof(RoleTypes.TenantAdminAdd)}");
			}

			if (status.HasErrors)
				return status;

			return status.SetResult(_lazyRoles.Value.Where(x => roleNames.Contains(x.RoleName)).ToList());
		}
	}
}