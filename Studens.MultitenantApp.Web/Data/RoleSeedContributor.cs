using Microsoft.EntityFrameworkCore;
using Rev.AuthPermissions.BaseCode;
using Rev.AuthPermissions.BaseCode.CommonCode;
using Rev.AuthPermissions.BaseCode.DataLayer.Classes;
using Rev.AuthPermissions.BaseCode.DataLayer.Classes.SupportTypes;
using Rev.AuthPermissions.BaseCode.DataLayer.EfCode;
using Rev.AuthPermissions.BaseCode.PermissionsCode;
using StatusGeneric;
using Studens.Data.Seed;

namespace Studens.MultitenantApp.Web.Data
{
	/// <summary>
	/// Run roles before users and tenants.
	/// </summary>
	[DataSeed(Order = -100)]
	public class RoleSeedContributor : IDataSeedContributor
	{
		protected AuthPermissionsDbContext DbContext { get; }
		private readonly Type _enumPermissionType;

		public RoleSeedContributor(AuthPermissionsDbContext dbContext, AuthPermissionsOptions options)
		{
			DbContext = dbContext;
			_enumPermissionType = options.InternalData.EnumPermissionsType;
		}

		public async Task SeedDataAsync()
		{
			if (await DbContext.RoleToPermissions.AnyAsync())
			{
				return;
			}

			var status = new StatusGenericHandler();

			foreach (var roleDefinition in UserSeedDataDefinition.RolesDefinition)
			{
				var perRoleStatus = new StatusGenericHandler();
				var permissionNames = roleDefinition.PermissionsCommaDelimited
					.Split(',').Select(x => x.Trim()).ToList();

				var roleType = roleDefinition.RoleType;
				//NOTE: If an advanced permission (i.e. has the display attribute has AutoGenerateFilter = true) is found the roleType is updated to HiddenFromTenant
				var packedPermissions = _enumPermissionType.PackPermissionsNamesWithValidation(permissionNames,
					x => perRoleStatus.AddError(
						$"The permission name '{x}' isn't a valid name in the {_enumPermissionType.Name} enum.",
						nameof(permissionNames).CamelToPascal()), () => roleType = RoleTypes.HiddenFromTenant);

				status.CombineStatuses(perRoleStatus);
				if (perRoleStatus.IsValid)
				{
					var role = new RoleToPermissions(roleDefinition.RoleName, roleDefinition.Description,
						packedPermissions, roleType);
					DbContext.Add(role);
				}
			}
			if (status.HasErrors)
				throw new InvalidOperationException(string.Join(", ", status.Errors));

			await DbContext.SaveChangesWithChecksAsync();
		}
	}
}