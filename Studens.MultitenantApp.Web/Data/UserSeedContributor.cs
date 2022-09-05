using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rev.AuthPermissions.AdminCode;
using Rev.AuthPermissions.BaseCode;
using Rev.AuthPermissions.BaseCode.CommonCode;
using Rev.AuthPermissions.BaseCode.DataLayer.Classes;
using Rev.AuthPermissions.BaseCode.DataLayer.EfCode;
using Rev.AuthPermissions.BulkLoadServices.Concrete.Internal;
using StatusGeneric;
using Studens.Data.Seed;

namespace Studens.MultitenantApp.Web.Data
{	
	public class UserSeedContributor : IDataSeedContributor
	{
		private const string DefaultPassword = "Password!1";

		protected UserManager<User> UserManager { get; }
		protected AuthPermissionsDbContext DbContext { get; }
		protected AuthPermissionsOptions Options { get; }

		public UserSeedContributor(UserManager<User> userManager, AuthPermissionsOptions options, AuthPermissionsDbContext dbContext)
		{
			UserManager = userManager;
			Options = options;
			DbContext = dbContext;
		}

		/// <summary>
		/// TODO: Seed users
		/// TODO: Make sure departments and positions are sedeed before users...Introduce ordering attribute
		/// </summary>
		public async Task SeedDataAsync()
		{
			var status = new StatusGenericHandler();
			var roles = await DbContext.RoleToPermissions.ToListAsync();
			var users = UserSeedDataDefinition.UsersRolesDefinition;

			for (int i = 0; i < users.Count; i++)
			{
				var userDefinition = users[i];
				var userStatus = new StatusGenericHandler();

				var rolesToPermissions = new List<RoleToPermissions>();
				userDefinition.RoleNamesCommaDelimited.DecodeCommaDelimitedNameWithCheck(0,
					(name, startOfName) =>
					{
						var roleToPermission = roles.SingleOrDefault(x => x.RoleName == name);
						if (roleToPermission == null)
							userStatus.AddError(userDefinition.RoleNamesCommaDelimited.FormErrorString(i, startOfName,
								$"The role {name} wasn't found in the auth database."));
						else
							rolesToPermissions.Add(roleToPermission);
					});

				Tenant? userTenant = null;
				if (Options.TenantType.IsMultiTenant() && !string.IsNullOrEmpty(userDefinition.TenantNameForDataKey))
				{
					userTenant = await DbContext.Tenants.SingleOrDefaultAsync(x => x.TenantName == userDefinition.TenantNameForDataKey);
					if (userTenant == null)
					{
						status.AddError(userDefinition.UserName.FormErrorString(i - 1, -1,
							$"The user {userDefinition.UserName} has a tenant name of {userDefinition.TenantNameForDataKey} which wasn't found in the auth database."));
						break;
					}
				}

				var userCreateStatus = User.CreateUser(Guid.NewGuid().ToString(), userDefinition.Email, userDefinition.UserName, rolesToPermissions, userTenant);
				userStatus.CombineStatuses(userCreateStatus);

				if (userStatus.HasErrors)
				{
					break;
				}

				var user = userCreateStatus.Result;
				user.EmailConfirmed = user.PhoneNumberConfirmed = true;

				await UserManager.CreateAsync(user, DefaultPassword);

				status.CombineStatuses(userStatus);
			}

			if (status.HasErrors)
				throw new InvalidOperationException(string.Join(", ", status.Errors));

			//var superAdministrator = CreateDefaultUser();
			//superAdministrator.Email = superAdministrator.UserName = "superadmin@ito.ba";

			//await UserManager.CreateAsync(superAdministrator, DefaultPassword);

			//var administrator = CreateDefaultUser();
			//administrator.Email = administrator.UserName = "app-admin@ito.ba";

			//await UserManager.CreateAsync(administrator, DefaultPassword);
		}		
	}
}