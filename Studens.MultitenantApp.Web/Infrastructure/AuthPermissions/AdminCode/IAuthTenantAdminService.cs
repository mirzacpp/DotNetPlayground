// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using Rev.AuthPermissions.BaseCode.DataLayer.Classes;
using StatusGeneric;

namespace Rev.AuthPermissions.AdminCode
{
	/// <summary>
	/// interface of the AuthP Tenant admin services
	/// </summary>
	public interface IAuthTenantAdminService
	{
		/// <summary>
		/// This simply returns a IQueryable of Tenants
		/// </summary>
		/// <returns>query on the AuthP database</returns>
		IQueryable<Tenant> QueryTenants();		

		/// <summary>
		/// This returns a list of all the RoleNames that can be applied to a Tenant
		/// </summary>
		/// <returns></returns>
		Task<List<string>> GetRoleNamesForTenantsAsync();

		/// <summary>
		/// This returns a tenant, with TenantRoles and its Parent but no children, that has the given TenantId
		/// </summary>
		/// <param name="tenantId">primary key of the tenant you are looking for</param>
		/// <returns>Status. If successful, then contains the Tenant</returns>
		Task<IStatusGeneric<Tenant>> GetTenantViaIdAsync(string tenantId);

		/// <summary>
		/// This adds a new, single level  Tenant
		/// </summary>
		/// <param name="tenantName">Name of the new single-level tenant (must be unique)</param>
		/// <param name="tenantRoleNames">Optional: List of tenant role names</param>
		/// <param name="hasOwnDb">Needed if sharding: Is true if this tenant has its own database, else false</param>
		/// <param name="databaseInfoName">This is the name of the database information in the shardingsettings file.</param>
		/// <returns>A status containing the <see cref="Tenant"/> class</returns>
		Task<IStatusGeneric<Tenant>> AddSingleTenantAsync(string tenantName, List<string> tenantRoleNames = null,
			bool? hasOwnDb = false, string databaseInfoName = null);

		/// <summary>
		/// This replaces the <see cref="Tenant.TenantRoles"/> in the tenant with <see param="tenantId"/> primary key
		/// </summary>
		/// <param name="tenantId">Primary key of the tenant to change</param>
		/// <param name="newTenantRoleNames">List of RoleName to replace the current tenant's <see cref="Tenant.TenantRoles"/></param>
		/// <returns></returns>
		Task<IStatusGeneric> UpdateTenantRolesAsync(string tenantId, List<string> newTenantRoleNames);

		/// <summary>
		/// This updates the name of this tenant to the <see param="newTenantLevelName"/>.
		/// This also means all the children underneath need to have their full name updated too
		/// This method uses the <see cref="ITenantChangeService"/> you provided via the <see cref="RegisterExtensions.RegisterTenantChangeService{TTenantChangeService}"/>
		/// to update the application's tenant data.
		/// </summary>
		/// <param name="tenantId">Primary key of the tenant to change</param>
		/// <param name="newTenantName">This is the new name for this tenant name</param>
		/// <returns></returns>
		Task<IStatusGeneric> UpdateTenantNameAsync(string tenantId, string newTenantName);

		/// <summary>
		/// This will delete the tenant (and all its children if the data is hierarchical) and uses the <see cref="ITenantChangeService"/>,
		/// but only if no AuthP user are linked to this tenant (it will return errors listing all the AuthP user that are linked to this tenant
		/// This method uses the <see cref="ITenantChangeService"/> you provided via the <see cref="RegisterExtensions.RegisterTenantChangeService{TTenantChangeService}"/>
		/// to delete the application's tenant data.
		/// NOTE: If the tenant is hierarchical, then it will delete the tenant and all of its child tenants
		/// </summary>
		/// <param name="tenantId">The primary key of the AuthP tenant to be deleted</param>
		/// <returns>Status returning the <see cref="ITenantChangeService"/> service, in case you want copy the delete data instead of deleting</returns>
		Task<IStatusGeneric<ITenantChangeService>> DeleteTenantAsync(string tenantId);

		/// <summary>
		/// This is used when sharding is enabled. It updates the tenant's <see cref="Tenant.DatabaseInfoName"/> and
		/// <see cref="Tenant.HasOwnDb"/> and calls the  <see cref="ITenantChangeService"/> <see cref="ITenantChangeService.MoveToDifferentDatabaseAsync"/>
		/// which moves the tenant data to another database and then deletes the the original tenant data.
		/// </summary>
		/// <param name="tenantToMoveId">The primary key of the AuthP tenant to be moved.
		///     NOTE: If its a hierarchical tenant, then the tenant must be the highest parent.</param>
		/// <param name="hasOwnDb">Says whether the new database will only hold this tenant</param>
		/// <param name="databaseInfoName">The name of the connection string in the ConnectionStrings part of the appsettings file</param>
		/// <returns>status</returns>
		Task<IStatusGeneric> MoveToDifferentDatabaseAsync(string tenantToMoveId,
			bool hasOwnDb, string databaseInfoName);
	}
}