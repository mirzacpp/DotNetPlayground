using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Rev.AuthPermissions.AdminCode;
using Rev.AuthPermissions.BaseCode.CommonCode;
using Rev.AuthPermissions.BaseCode.DataLayer.Classes;
using Studens.MultitenantApp.Web.Data;

namespace Studens.MultitenantApp.Web.Permissions
{
	public class TenantChangeService : ITenantChangeService
	{
		private readonly ITenantDbContextFactory _tenantDbContextFactory;
		private readonly IUsersAdminService _usersAdminService;
		private readonly ILogger _logger;

		/// <summary>
		/// This allows the tenantId of the deleted tenant to be returned.
		/// This is useful if you want to soft delete the data
		/// </summary>
		public int DeletedTenantId { get; private set; }

		public TenantChangeService(ILogger<TenantChangeService> logger,
			ITenantDbContextFactory dbContextFactory,
			IUsersAdminService usersAdminService)
		{
			_logger = logger;
			_tenantDbContextFactory = dbContextFactory;
			_usersAdminService = usersAdminService;
		}

		/// <summary>
		/// This creates a <see cref="CompanyTenant"/> in the given database
		/// </summary>
		/// <param name="tenant"></param>
		/// <returns>Null if no errors, otherwise string is shown as an error to the user</returns>
		public async Task<string> CreateNewTenantAsync(Tenant tenant)
		{
			using var context = _tenantDbContextFactory.CreateTenantsDbContext(null, tenant.GetTenantDataKey());
			using var tenantsContext = _tenantDbContextFactory.CreateTenantsDbContext(tenant.DatabaseInfoName, null);

			if (tenantsContext == null)
				return $"There is no connection string with the name {tenant.DatabaseInfoName}.";

			var databaseError = await CheckDatabaseAndPossibleMigrate(tenantsContext, tenant, true);
			if (databaseError != null)
				return databaseError;

			// TODO: When migrated we should create default tenant admin with provided email...
			// We will assign user to a tenant admin role to enable tenant management.

			//var roles = await context.RoleToPermissions.Where(y => y.RoleType == RoleTypes.TenantAutoAdd).ToListAsync();

			//var user = User.CreateUser(Guid.NewGuid().ToString(),
			//	$"app-admin-{tenant.TenantName}@app.com",
			//	null,
			//new List<RoleToPermissions>(),
			//tenant);

			await _usersAdminService.AddNewUserAsync(Guid.NewGuid().ToString(),
				$"app-admin-{tenant.TenantName}@app.com",
				null!,
			new List<string> { ApplicationRoles.Admin },
			tenant.TenantName);
			
			//tenantsContext.Users.Add(user.Result);
			//await tenantsContext.SaveChangesAsync();

			return null;
		}

		public async Task<string> SingleTenantUpdateNameAsync(Tenant tenant)
		{
			using var context = _tenantDbContextFactory.CreateTenantsDbContext(tenant.DatabaseInfoName, tenant.GetTenantDataKey());
			if (context == null)
				return $"There is no connection string with the name {tenant.DatabaseInfoName}.";

			return null;
		}

		public async Task<string> SingleTenantDeleteAsync(Tenant tenant)
		{
			using var context = _tenantDbContextFactory.CreateTenantsDbContext(tenant.DatabaseInfoName, tenant.GetTenantDataKey());
			if (context == null)
				return $"There is no connection string with the name {tenant.DatabaseInfoName}.";

			return null;
		}

		/// <summary>
		/// This method can be quite complicated. It has to
		/// 1. Copy the data from the previous database into the new database
		/// 2. Delete the old data
		/// These two steps have to be done within a transaction, so that a failure to delete the old data will roll back the copy.
		/// </summary>
		/// <param name="oldDatabaseInfoName"></param>
		/// <param name="oldDataKey"></param>
		/// <param name="updatedTenant"></param>
		/// <returns></returns>
		public async Task<string> MoveToDifferentDatabaseAsync(string oldDatabaseInfoName, string oldDataKey, Tenant updatedTenant)
		{
			//NOTE: The oldContext and newContext have the correct DataKey so you don't have to use IgnoreQueryFilters.
			using var oldContext = _tenantDbContextFactory.CreateTenantsDbContext(oldDatabaseInfoName, oldDataKey);

			if (oldContext == null)
				return $"There is no connection string with the name {oldDatabaseInfoName}.";

			using var newContext = _tenantDbContextFactory.CreateTenantsDbContext(updatedTenant.DatabaseInfoName, updatedTenant.GetTenantDataKey());

			if (newContext == null)
				return $"There is no connection string with the name {updatedTenant.DatabaseInfoName}.";

			var databaseError = await CheckDatabaseAndPossibleMigrate(newContext, updatedTenant, true);
			if (databaseError != null)
				return databaseError;

			return null;
		}

		//--------------------------------------------------
		//private methods / classes

		/// <summary>
		/// This check is a database is there
		/// </summary>
		/// <param name="context">The context for the new database</param>
		/// <param name="tenant"></param>
		/// <param name="migrateEvenIfNoDb">If using local SQL server, Migrate will create the database.
		/// That doesn't work on Azure databases</param>
		/// <returns></returns>
		private static async Task<string?> CheckDatabaseAndPossibleMigrate(ApplicationDbContext context, Tenant tenant,
			bool migrateEvenIfNoDb)
		{
			//Thanks to https://stackoverflow.com/questions/33911316/entity-framework-core-how-to-check-if-database-exists
			//There are various options to detect if a database is there - this seems the clearest
			if (!await context.Database.CanConnectAsync())
			{
				//The database doesn't exist
				if (migrateEvenIfNoDb)
					await context.Database.MigrateAsync();
				else
				{
					return $"The database defined by the connection string '{tenant.DatabaseInfoName}' doesn't exist.";
				}
			}
			else if (!await context.Database.GetService<IRelationalDatabaseCreator>().HasTablesAsync())
				//The database exists but needs migrating
				await context.Database.MigrateAsync();

			return null;
		}

		private async Task DeleteTenantData(string dataKey, ApplicationDbContext context)
		{
			await Task.CompletedTask;
		}
	}
}