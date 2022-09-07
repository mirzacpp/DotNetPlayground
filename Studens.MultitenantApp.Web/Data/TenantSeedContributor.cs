using Microsoft.EntityFrameworkCore;
using Rev.AuthPermissions.BaseCode;
using Rev.AuthPermissions.BaseCode.DataLayer.Classes;
using Rev.AuthPermissions.BaseCode.DataLayer.Classes.SupportTypes;
using Rev.AuthPermissions.BaseCode.DataLayer.EfCode;
using Rev.AuthPermissions.BaseCode.SetupCode;
using StatusGeneric;
using Studens.Data.Seed;

namespace Studens.MultitenantApp.Web.Data
{
	/// <summary>
	/// Seed before users but after roles.
	/// </summary>
	[DataSeed(Order = -50)]
	public class TenantSeedContributor : IDataSeedContributor
	{
		protected AuthPermissionsDbContext DbContext { get; }
		private List<RoleToPermissions> _roles;
		protected AuthPermissionsOptions Options { get; }

		public TenantSeedContributor(AuthPermissionsDbContext dbContext, AuthPermissionsOptions options)
		{
			DbContext = dbContext;
			Options = options;
		}

		public async Task SeedDataAsync()
		{
			if (await DbContext.Set<Tenant>().AnyAsync())
			{
				return;
			}

			// Grab all roles prepared for tenant
			_roles = await DbContext.Set<RoleToPermissions>()
				.Where(x => x.RoleType == RoleTypes.TenantAutoAdd || x.RoleType == RoleTypes.TenantAdminAdd)
				.ToListAsync();

			var status = new StatusGenericHandler();

			foreach (var tenantDefinition in UserSeedDataDefinition.TenantDefinition)
			{
				var rolesStatus = GetCheckTenantRoles(tenantDefinition.TenantRolesCommaDelimited, tenantDefinition.TenantName);
				status.CombineStatuses(rolesStatus);
				var tenantStatus = Tenant.CreateSingleTenant(Guid.NewGuid().ToString(), tenantDefinition.TenantName, rolesStatus.Result);

				if (status.CombineStatuses(tenantStatus).IsValid)
				{
					if ((Options.TenantType & TenantTypes.AddSharding) != 0)
					{
						tenantStatus.Result.UpdateShardingState(Options.ShardingDefaultDatabaseInfoName, false);
					}
					DbContext.Add(tenantStatus.Result);
				}
			}

			if (status.HasErrors)
				throw new InvalidOperationException(string.Join(", ", status.Errors));

			await DbContext.SaveChangesWithChecksAsync();
		}

		private IStatusGeneric<List<RoleToPermissions>> GetCheckTenantRoles(string tenantRolesCommaDelimited, string fullTenantName)
		{
			var status = new StatusGenericHandler<List<RoleToPermissions>>();

			if (tenantRolesCommaDelimited == null)
				return status.SetResult(null);

			var roleNames = tenantRolesCommaDelimited.Split(',').Select(x => x.Trim())
				.Distinct().ToList();

			//check provided role names are in the database
			var notFoundNames = roleNames
				.Where(x => !_roles.Select(y => y.RoleName).Contains(x)).ToList();

			foreach (var notFoundName in notFoundNames)
			{
				status.AddError($"Tenant '{fullTenantName}': the role called '{notFoundName}' was not found. Either it is misspent or " +
								$"the {nameof(RoleToPermissions.RoleType)} must be {nameof(RoleTypes.TenantAutoAdd)} or {nameof(RoleTypes.TenantAdminAdd)}");
			}

			if (status.HasErrors)
				return status;

			return status.SetResult(_roles.Where(x => roleNames.Contains(x.RoleName)).ToList());
		}
	}
}