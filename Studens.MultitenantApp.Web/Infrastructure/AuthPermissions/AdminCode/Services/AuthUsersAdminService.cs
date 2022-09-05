// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rev.AuthPermissions.BaseCode;
using Rev.AuthPermissions.BaseCode.CommonCode;
using Rev.AuthPermissions.BaseCode.DataLayer.Classes;
using Rev.AuthPermissions.BaseCode.DataLayer.Classes.SupportTypes;
using Rev.AuthPermissions.BaseCode.DataLayer.EfCode;
using StatusGeneric;

namespace Rev.AuthPermissions.AdminCode.Services
{
	/// <summary>
	/// This provides CRUD access to the AuthP's Users
	/// </summary>
	public class UsersAdminService : IUsersAdminService
	{
		private readonly AuthPermissionsDbContext _context;
		private readonly UserManager<User> _userManager;
		private readonly bool _isMultiTenant;

		/// <summary>
		/// ctor
		/// </summary>
		/// <param name="context"></param>
		/// <param name="syncAuthenticationUsersFactory">A factory to create an authentication sync provider</param>
		/// <param name="options">auth options</param>
		public UsersAdminService(AuthPermissionsDbContext context,
			AuthPermissionsOptions options,
			UserManager<User> userManager)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_isMultiTenant = options.TenantType.IsMultiTenant();
			_userManager = userManager;
		}

		/// <summary>
		/// This returns a IQueryable of User, with optional filtering by dataKey (useful for tenant admin)
		/// </summary>
		/// <param name="dataKey">optional dataKey. If provided then it only returns Users that fall within that dataKey</param>
		/// <returns>query on the database</returns>
		public IQueryable<User> QueryUsers(string dataKey = null)
		{
			return dataKey == null
				? _context.Users
				: _context.Users.Where(x => x.TenantId.Equals(dataKey));
		}

		/// <summary>
		/// Finds a User via its UserId. Returns a status with an error if not found
		/// </summary>
		/// <param name="userId"></param>
		/// <returns>Status containing the User with UserRoles and UserTenant, or errors</returns>
		public async Task<IStatusGeneric<User>> FindUserByUserIdAsync(string userId)
		{
			if (userId == null) throw new ArgumentNullException(nameof(userId));
			var status = new StatusGenericHandler<User>();

			var User = await _context.Users
				.Include(x => x.UserRoles)
				.Include(x => x.UserTenant)
				.SingleOrDefaultAsync(x => x.Id == userId);

			if (User == null)
				status.AddError("Could not find the AuthP User you asked for.", nameof(userId).CamelToPascal());

			return status.SetResult(User);
		}

		/// <summary>
		/// Find a User via its email. Returns a status with an error if not found
		/// </summary>
		/// <param name="email"></param>
		/// <returns>Status containing the User with UserRoles and UserTenant, or errors</returns>
		public async Task<IStatusGeneric<User>> FindUserByEmailAsync(string email)
		{
			if (email == null) throw new ArgumentNullException(nameof(email));
			var status = new StatusGenericHandler<User>();

			email = email.Trim().ToLower();

			var User = await _context.Users
				.Include(x => x.UserRoles)
				.Include(x => x.UserTenant)
				.SingleOrDefaultAsync(x => x.Email == email);

			if (User == null)
				status.AddError($"Could not find the AuthP User with the email of {email}.",
					nameof(email).CamelToPascal());

			return status.SetResult(User);
		}

		/// <summary>
		/// This will changes the <see cref="User.IsDisabled"/> for the user with the given userId
		/// A disabled user causes the <see cref="ClaimsCalculator"/> to not add any AuthP claims to the user on login
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="isDisabled">New setting for the <see cref="User.IsDisabled"/></param>
		/// <returns>Status containing the User with UserRoles and UserTenant, or errors</returns>
		public async Task<IStatusGeneric> UpdateDisabledAsync(string userId, bool isDisabled)
		{
			if (userId == null) throw new ArgumentNullException(nameof(userId));
			var status = new StatusGenericHandler
			{ Message = $"Successfully changed the user's {nameof(User.IsDisabled)} to {isDisabled}" };

			var user = await _context.Users
				.SingleOrDefaultAsync(x => x.Id == userId);

			if (user == null)
				return status.AddError("Could not find the AuthP User you asked for.", nameof(userId).CamelToPascal());

			user.UpdateIsDisabled(isDisabled);
			status.CombineStatuses(await _context.SaveChangesWithChecksAsync());

			return status;
		}

		/// <summary>
		/// This returns a list of all the RoleNames that can be applied to the User
		/// Doesn't work properly when used in a create, as the user's tenant hasn't be set
		/// </summary>
		/// <param name="userId">UserId of the user you are updating. Only needed in multi-tenant applications </param>
		/// <param name="addNone">Defaults to true, with will add the <see cref="CommonConstants.EmptyItemName"/> at the start.
		/// This is useful for selecting no roles</param>
		/// <returns></returns>
		public async Task<List<string>> GetRoleNamesForUsersAsync(string userId = null, bool addNone = true)
		{
			List<string> InsertEmptyNameIfNeeded(List<string> localRoleNames)
			{
				if (addNone)
					localRoleNames.Insert(0, CommonConstants.EmptyItemName);
				return localRoleNames;
			}

			if (!_isMultiTenant)
				return InsertEmptyNameIfNeeded(await _context.RoleToPermissions
					.Select(x => x.RoleName).ToListAsync());

			if (userId == null)
				throw new ArgumentNullException(nameof(userId), "You must be logged in to use this feature.");

			//multi-tenant version has to filter out the roles from users that have a tenant
			var userWithTenantRoles = await _context.Users
				.Include(x => x.UserTenant)
				.ThenInclude(x => x.TenantRoles)
				.SingleAsync(x => x.Id == userId);

			if (userWithTenantRoles.UserTenant == null)
				//Its an app-level user so return all non-tenant roles
				return InsertEmptyNameIfNeeded(await _context.RoleToPermissions
					.Where(x => x.RoleType == RoleTypes.Normal || x.RoleType == RoleTypes.HiddenFromTenant)
					.Select(x => x.RoleName)
					.ToListAsync());

			//its a tenant-level user, so return Normal and TenantAdminAdd
			//First find the Normal Roles
			var roleNames = await _context.RoleToPermissions
				.Where(x => x.RoleType == RoleTypes.Normal)
				.Select(x => x.RoleName)
				.ToListAsync();

			//Then add any TenantAdminAdd roles in the tenant's TenantRoles
			roleNames.AddRange(userWithTenantRoles.UserTenant.TenantRoles
				.Where(x => x.RoleType == RoleTypes.TenantAdminAdd).Select(x => x.RoleName));

			return InsertEmptyNameIfNeeded(roleNames);
		}

		/// <summary>
		/// This returns all the tenant full names
		/// </summary>
		/// <returns></returns>
		public async Task<List<string>> GetAllTenantNamesAsync()
		{
			return await _context.Tenants.Select(x => x.TenantName).ToListAsync();
		}

		/// <summary>
		/// This adds a new AuthUse to the database
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="email">if not null, then checked to be a valid email</param>
		/// <param name="userName"></param>
		/// <param name="roleNames">The rolenames of this user - if null then assumes no roles</param>
		/// <param name="tenantName">optional: full name of the tenant</param>
		/// <returns></returns>
		public async Task<IStatusGeneric> AddNewUserAsync(string userId, string email,
			string userName, List<string> roleNames, string tenantName = null)
		{
			var status = new StatusGenericHandler
			{ Message = $"Successfully added a User with the name {userName ?? email}" };

			if (email != null && !email.IsValidEmail())
				status.AddError($"The email '{email}' is not a valid email.");

			//Find the tenant
			var foundTenant = string.IsNullOrEmpty(tenantName) || tenantName == CommonConstants.EmptyItemName
				? null
				: await _context.Tenants.Include(x => x.TenantRoles)
					.SingleOrDefaultAsync(x => x.TenantName == tenantName);
			if (!string.IsNullOrEmpty(tenantName) && tenantName != CommonConstants.EmptyItemName && foundTenant == null)
				status.AddError($"A tenant with the name '{tenantName}' wasn't found.");

			//Find/check the roles
			var rolesStatus = await FindCheckRolesAreValidForUserAsync(roleNames, foundTenant, userName ?? email);

			if (status.CombineStatuses(rolesStatus).HasErrors)
				return status;

			var UserStatus = User.CreateUser(userId, email, userName, rolesStatus.Result, foundTenant);
			if (status.CombineStatuses(UserStatus).HasErrors)
				return status;

			//Register with user manager
			// We will need to combine status with identity result
			await _userManager.CreateAsync(UserStatus.Result);

			//_context.Add(UserStatus.Result);
			//status.CombineStatuses(await _context.SaveChangesWithChecksAsync());

			return status;
		}

		/// <summary>
		/// This update an existing User. This method is designed so you only have to provide data for the parts you want to update,
		/// i.e. if a parameter is null, then it keeps the original setting. The only odd one out is the tenantName,
		/// where you have to provide the <see cref="CommonConstants.EmptyItemName"/> value to remove the tenant.
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="email">Either provide a email or null. if null, then uses the current user's email</param>
		/// <param name="userName">Either provide a userName or null. if null, then uses the current user's userName</param>
		/// <param name="roleNames">Either a list of rolenames or null. If null, then keeps its current rolenames.
		/// If the rolesNames collection only contains a single entry with the value <see cref="CommonConstants.EmptyItemName"/>,
		/// then the roles will be set to an empty collection.</param>
		/// <param name="tenantName">If null, then keeps current tenant. If it is <see cref="CommonConstants.EmptyItemName"/> it will remove a tenant link.
		/// Otherwise the user will be linked to the tenant with that name.</param>
		/// <returns>status</returns>
		public async Task<IStatusGeneric> UpdateUserAsync(string userId,
			string email = null, string userName = null, List<string> roleNames = null, string tenantName = null)
		{
			if (userId == null) throw new ArgumentNullException(nameof(userId));

			var status = new StatusGenericHandler();

			var foundUserStatus = await FindUserByUserIdAsync(userId);
			if (status.CombineStatuses(foundUserStatus).HasErrors)
				return status;

			email ??= foundUserStatus.Result.Email;
			userName ??= foundUserStatus.Result.UserName;

			status.Message = $"Successfully updated a User with the name {userName ?? email}";

			var UserToUpdate = foundUserStatus.Result;

			if (email != null && !email.IsValidEmail())
				status.AddError($"The email '{email}' is not a valid email.");

			//Now we update the existing User's email and userName
			UserToUpdate.ChangeUserNameAndEmailWithChecks(email, userName);

			//Get current tenant as roleNames needs tenant
			var foundTenant = foundUserStatus.Result.UserTenant;
			if (foundTenant != null && tenantName == null && roleNames != null)
				//You are going to update the roles and you aren't changing the tenant, then you need to load the TenantRoles
				await _context.Entry(foundTenant)
					.Collection(x => x.TenantRoles).LoadAsync();

			//If tenantName isn't null, then update the user's tenant
			if (tenantName != null)
			{
				//Find the tenant
				foundTenant = string.IsNullOrEmpty(tenantName) || tenantName == CommonConstants.EmptyItemName
					? null
					: await _context.Tenants.Include(x => x.TenantRoles)
						.SingleOrDefaultAsync(x => x.TenantName == tenantName);

				if (!string.IsNullOrEmpty(tenantName) && tenantName != CommonConstants.EmptyItemName && foundTenant == null)
					return status.AddError($"A tenant with the name '{tenantName}' wasn't found.");

				UserToUpdate.UpdateUserTenant(foundTenant);
			}

			//If rolenames isn't null, then update with new RoleNames
			if (roleNames != null)
			{
				var updatedRoles = new List<RoleToPermissions>();
				if (!(roleNames.Count == 1 && roleNames.Single() == CommonConstants.EmptyItemName))
				{
					//Find/check Roles
					var rolesStatus = await FindCheckRolesAreValidForUserAsync(roleNames, foundTenant, userName ?? email);

					if (status.CombineStatuses(rolesStatus).HasErrors)
						return status;

					updatedRoles = rolesStatus.Result;
				}
				UserToUpdate.ReplaceAllRoles(updatedRoles);
			}

			status.CombineStatuses(await _context.SaveChangesWithChecksAsync());

			return status;
		}

		/// <summary>
		/// This will delete the User with the given userId
		/// </summary>
		/// <param name="userId"></param>
		/// <returns>status</returns>
		public async Task<IStatusGeneric> DeleteUserAsync(string userId)
		{
			var status = new StatusGenericHandler();

			var User = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId);

			if (User == null)
				return status.AddError("Could not find the user you were looking for.", nameof(userId).CamelToPascal());

			_context.Remove(User);
			status.CombineStatuses(await _context.SaveChangesWithChecksAsync());

			status.Message = $"Successfully deleted the user {User.UserName ?? User.Email}.";

			return status;
		}

		//---------------------------------------------------------
		// private methods

		/// <summary>
		/// This finds and checks that the roles are valid for this type of user and tenant
		/// </summary>
		/// <param name="roleNames"></param>
		/// <param name="usersTenant">NOTE: must include the tenant's roles</param>
		/// <param name="userName">name/email of the user</param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		private async Task<IStatusGeneric<List<RoleToPermissions>>> FindCheckRolesAreValidForUserAsync(List<string> roleNames, Tenant usersTenant, string userName)
		{
			var status = new StatusGenericHandler<List<RoleToPermissions>>();

			var foundRoles = roleNames?.Any() == true
				? await _context.RoleToPermissions
					.Where(x => roleNames.Contains(x.RoleName))
					.ToListAsync()
				: new List<RoleToPermissions>();
			if (foundRoles.Count != (roleNames?.Count ?? 0))
			{
				foreach (var badRoleName in roleNames.Where(x => !foundRoles.Select(y => y.RoleName).Contains(x)))
					status.AddError($"The Role '{badRoleName}' was not found in the lists of Roles.");
			}

			//Check that the Roles are allowed for this user
			foreach (var foundRole in foundRoles)
			{
				if (usersTenant == null && foundRole.RoleType == RoleTypes.TenantAdminAdd)
					status.AddError($"The role '{foundRole.RoleName}' isn't allowed to a non-tenant user.");

				if (usersTenant != null && foundRole.RoleType == RoleTypes.HiddenFromTenant)
					status.AddError($"The role '{foundRole.RoleName}' isn't allowed to tenant user.");

				if (usersTenant != null && foundRole.RoleType == RoleTypes.TenantAdminAdd
					&& !usersTenant.TenantRoles.Contains(foundRole))
					status.AddError($"The role '{foundRole.RoleName}' wasn't found in the tenant '{usersTenant.TenantName}' tenant roles.");
			}

			status.SetResult(foundRoles);
			return status;
		}
	}
}