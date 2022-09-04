// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;
using Rev.AuthPermissions.BaseCode;
using Rev.AuthPermissions.BaseCode.CommonCode;
using Rev.AuthPermissions.BaseCode.DataLayer.Classes;
using Rev.AuthPermissions.BaseCode.DataLayer.Classes.SupportTypes;
using Rev.AuthPermissions.BaseCode.DataLayer.EfCode;
using Rev.AuthPermissions.BaseCode.SetupCode;
using Rev.AuthPermissions.SetupCode.Factories;
using StatusGeneric;
using Studens.MultitenantApp.Web.Data;
using System.Data;

namespace Rev.AuthPermissions.AdminCode.Services
{
	/// <summary>
	/// This provides CRUD services for tenants
	/// </summary>
	public class AuthTenantAdminService : IAuthTenantAdminService
	{
		private readonly ApplicationDbContext _context;
		private readonly IAuthPServiceFactory<ITenantChangeService> _tenantChangeServiceFactory;
		private readonly AuthPermissionsOptions _options;
		private readonly ILogger _logger;

		private readonly TenantTypes _tenantType;

		/// <summary>
		/// ctor
		/// </summary>
		/// <param name="context"></param>
		/// <param name="options"></param>
		/// <param name="tenantChangeServiceFactory"></param>
		/// <param name="logger"></param>
		public AuthTenantAdminService(ApplicationDbContext context,
			AuthPermissionsOptions options,
			IAuthPServiceFactory<ITenantChangeService> tenantChangeServiceFactory,
			ILogger<AuthTenantAdminService> logger)
		{
			_context = context;
			_options = options;
			_tenantChangeServiceFactory = tenantChangeServiceFactory;
			_logger = logger;

			_tenantType = options.TenantType;
		}

		/// <summary>
		/// This simply returns a IQueryable of Tenants
		/// </summary>
		/// <returns>query on the AuthP database</returns>
		public IQueryable<Tenant> QueryTenants()
		{
			return _context.Tenants;
		}

		/// <summary>
		/// This returns a list of all the RoleNames that can be applied to a Tenant
		/// </summary>
		/// <returns></returns>
		public async Task<List<string>> GetRoleNamesForTenantsAsync()
		{
			return await _context.RoleToPermissions
				.Where(x => x.RoleType == RoleTypes.TenantAutoAdd || x.RoleType == RoleTypes.TenantAdminAdd)
				.Select(x => x.RoleName)
				.ToListAsync();
		}

		/// <summary>
		/// This returns a tenant, with TenantRoles and its Parent but no children, that has the given TenantId
		/// </summary>
		/// <param name="tenantId">primary key of the tenant you are looking for</param>
		/// <returns>Status. If successful, then contains the Tenant</returns>
		public async Task<IStatusGeneric<Tenant>> GetTenantViaIdAsync(string tenantId)
		{
			var status = new StatusGenericHandler<Tenant>();

			var result = await _context.Tenants
				.Include(x => x.TenantRoles)
				.SingleOrDefaultAsync(x => x.TenantId == tenantId);
			return result == null
				? status.AddError("Could not find the tenant you were looking for.")
				: status.SetResult(result);
		}

		/// <summary>
		/// This adds a new, single level Tenant
		/// </summary>
		/// <param name="tenantName">Name of the new single-level tenant (must be unique)</param>
		/// <param name="tenantRoleNames">Optional: List of tenant role names</param>
		/// <param name="hasOwnDb">Needed if sharding: Is true if this tenant has its own database, else false</param>
		/// <param name="databaseInfoName">This is the name of the database information in the shardingsettings file.</param>
		/// <returns>A status containing the <see cref="Tenant"/> class</returns>
		public async Task<IStatusGeneric<Tenant>> AddSingleTenantAsync(string tenantName, List<string> tenantRoleNames = null,
			bool? hasOwnDb = null, string databaseInfoName = null)
		{
			var status = new StatusGenericHandler<Tenant> { Message = $"Successfully added the new tenant {tenantName}." };

			if (!_tenantType.IsSingleLevel())
				throw new AuthPermissionsException(
					$"You cannot add a single tenant because the tenant configuration is {_tenantType}");

			var tenantChangeService = _tenantChangeServiceFactory.GetService();

			await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
			try
			{
				var tenantRolesStatus = await GetRolesWithChecksAsync(tenantRoleNames);
				status.CombineStatuses(tenantRolesStatus);
				//TODO: Insert guid generator
				var newTenantStatus = Tenant.CreateSingleTenant(Guid.NewGuid().ToString(), tenantName, tenantRolesStatus.Result);
				status.SetResult(newTenantStatus.Result);

				if (status.CombineStatuses(newTenantStatus).HasErrors)
					return status;

				if (_tenantType.IsSharding())
				{
					if (hasOwnDb == null)
						status.AddError($"The {nameof(hasOwnDb)} parameter must be set to true or false when sharding is turned on.",
							nameof(hasOwnDb).CamelToPascal());
					else
						status.CombineStatuses(await CheckHasOwnDbIsValidAsync((bool)hasOwnDb, databaseInfoName));

					if (status.HasErrors)
						return status;

					newTenantStatus.Result.UpdateShardingState(
						databaseInfoName ?? _options.ShardingDefaultDatabaseInfoName,
						(bool)hasOwnDb);
				}

				_context.Add(newTenantStatus.Result);
				status.CombineStatuses(await _context.SaveChangesWithChecksAsync());

				if (status.HasErrors)
					return status;

				var errorString = await tenantChangeService.CreateNewTenantAsync(newTenantStatus.Result);
				if (errorString != null)
					return status.AddError(errorString);

				await transaction.CommitAsync();
			}
			catch (Exception e)
			{
				if (_logger == null)
					throw;

				_logger.LogError(e, $"Failed to {e.Message}");
				return status.AddError(
					"The attempt to create a tenant failed with a system error. Please contact the admin team.");
			}

			return status;
		}

		/// <summary>
		/// This replaces the <see cref="Tenant.TenantRoles"/> in the tenant with <see param="tenantId"/> primary key
		/// </summary>
		/// <param name="tenantId">Primary key of the tenant to change</param>
		/// <param name="newTenantRoleNames">List of RoleName to replace the current tenant's <see cref="Tenant.TenantRoles"/></param>
		/// <returns></returns>
		public async Task<IStatusGeneric> UpdateTenantRolesAsync(string tenantId, List<string> newTenantRoleNames)
		{
			if (!_tenantType.IsMultiTenant())
				throw new AuthPermissionsException(
					$"You must set the {nameof(AuthPermissionsOptions.TenantType)} parameter in the AuthP's options");

			var status = new StatusGenericHandler { Message = "Successfully updated the tenant's Roles." };

			var tenant = await _context.Tenants.Include(x => x.TenantRoles)
				.SingleOrDefaultAsync(x => x.TenantId == tenantId);

			if (tenant == null)
				return status.AddError("Could not find the tenant you were looking for.");

			var tenantRolesStatus = await GetRolesWithChecksAsync(newTenantRoleNames);
			if (status.CombineStatuses(tenantRolesStatus).HasErrors)
				return status;

			var updateStatus = tenant.UpdateTenantRoles(tenantRolesStatus.Result);
			if (updateStatus.HasErrors)
				return updateStatus;

			return await _context.SaveChangesWithChecksAsync();
		}

		/// <summary>
		/// This updates the name of this tenant to the <see param="newTenantLevelName"/>.
		/// This also means all the children underneath need to have their full name updated too
		/// This method uses the <see cref="ITenantChangeService"/> you provided via the <see cref="RegisterExtensions.RegisterTenantChangeService"/>
		/// to update the application's tenant data.
		/// </summary>
		/// <param name="tenantId">Primary key of the tenant to change</param>
		/// <param name="newTenantName">This is the new name for this tenant name</param>
		/// <returns></returns>
		public async Task<IStatusGeneric> UpdateTenantNameAsync(string tenantId, string newTenantName)
		{
			var status = new StatusGenericHandler
			{ Message = $"Successfully updated the tenant's name to {newTenantName}." };

			if (string.IsNullOrEmpty(newTenantName))
				return status.AddError("The new name was empty", nameof(newTenantName).CamelToPascal());
			if (newTenantName.Contains('|'))
				return status.AddError(
					"The tenant name must not contain the character '|' because that character is used to separate the names in the hierarchical order",
						nameof(newTenantName).CamelToPascal());

			var tenantChangeService = _tenantChangeServiceFactory.GetService();

			using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
			try
			{
				var tenant = await _context.Tenants
					.SingleOrDefaultAsync(x => x.TenantId == tenantId);

				if (tenant == null)
					return status.AddError("Could not find the tenant you were looking for.");

				tenant.UpdateTenantName(newTenantName);

				var errorString = await tenantChangeService.SingleTenantUpdateNameAsync(tenant);
				if (errorString != null)
					return status.AddError(errorString);

				status.CombineStatuses(await _context.SaveChangesWithChecksAsync());

				if (status.IsValid)
					await transaction.CommitAsync();
			}
			catch (Exception e)
			{
				if (_logger == null)
					throw;

				_logger.LogError(e, $"Failed to {e.Message}");
				return status.AddError(
					"The attempt to delete a tenant failed with a system error. Please contact the admin team.");
			}
			status.Message = $"Successfully updated the tenant name to '{newTenantName}'.";

			return status;
		}

		/// <summary>
		/// This will delete the tenant (and all its children if the data is hierarchical) and uses the <see cref="ITenantChangeService"/>,
		/// but only if no AuthP user are linked to this tenant (it will return errors listing all the AuthP user that are linked to this tenant
		/// This method uses the <see cref="ITenantChangeService"/> you provided via the <see cref="RegisterExtensions.RegisterTenantChangeService{TTenantChangeService}"/>
		/// to delete the application's tenant data.
		/// </summary>
		/// <returns>Status returning the <see cref="ITenantChangeService"/> service, in case you want copy the delete data instead of deleting</returns>
		public async Task<IStatusGeneric<ITenantChangeService>> DeleteTenantAsync(string tenantId)
		{
			var status = new StatusGenericHandler<ITenantChangeService>();
			string message;

			var tenantChangeService = _tenantChangeServiceFactory.GetService();
			status.SetResult(tenantChangeService);

			using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
			try
			{
				var tenantToDelete = await _context.Tenants
					.SingleOrDefaultAsync(x => x.TenantId == tenantId);

				if (tenantToDelete == null)
					return status.AddError("Could not find the tenant you were looking for.");

				var allTenantIdsAffectedByThisDelete = await _context.Tenants
					.Where(x => x.TenantName.StartsWith(tenantToDelete.TenantName))
					.Select(x => x.TenantId)
					.ToListAsync();

				var usersOfThisTenant = await _context.Users
					.Where(x => allTenantIdsAffectedByThisDelete.Contains(x.TenantId ?? null))
					.Select(x => x.UserName ?? x.Email)
					.ToListAsync();

				var tenantOrChildren = allTenantIdsAffectedByThisDelete.Count > 1
					? "tenant or its children tenants are"
					: "tenant is";
				if (usersOfThisTenant.Any())
					usersOfThisTenant.ForEach(x =>
						status.AddError(
							$"This delete is aborted because this {tenantOrChildren} linked to the user '{x}'."));

				if (status.HasErrors)
					return status;

				message = $"Successfully deleted the tenant called '{tenantToDelete.TenantName}'";

				//delete the tenant that the user defines
				var mainError = await tenantChangeService.SingleTenantDeleteAsync(tenantToDelete);
				if (mainError != null)
					return status.AddError(mainError);
				_context.Remove(tenantToDelete);

				status.CombineStatuses(await _context.SaveChangesWithChecksAsync());

				if (status.IsValid)
					await transaction.CommitAsync();
			}
			catch (Exception e)
			{
				if (_logger == null)
					throw;

				_logger.LogError(e, $"Failed to {e.Message}");
				return status.AddError(
					"The attempt to delete a tenant failed with a system error. Please contact the admin team.");
			}

			status.Message = message + ".";
			return status;
		}

		/// <summary>
		/// This is used when sharding is enabled. It updates the tenant's <see cref="Tenant.DatabaseInfoName"/> and
		/// <see cref="Tenant.HasOwnDb"/> and calls the  <see cref="ITenantChangeService"/> <see cref="ITenantChangeService.MoveToDifferentDatabaseAsync"/>
		/// which moves the tenant data to another database and then deletes the the original tenant data.
		/// NOTE: You can change the <see cref="Tenant.HasOwnDb"/> by calling this method with no change to the <see cref="Tenant.DatabaseInfoName"/>.
		/// </summary>
		/// <param name="tenantToMoveId">The primary key of the AuthP tenant to be moved.
		///     NOTE: If its a hierarchical tenant, then the tenant must be the highest parent.</param>
		/// <param name="hasOwnDb">Says whether the new database will only hold this tenant</param>
		/// <param name="databaseInfoName">This is the name of the database information in the shardingsettings file.</param>
		/// <returns>status</returns>
		public async Task<IStatusGeneric> MoveToDifferentDatabaseAsync(string tenantToMoveId, bool hasOwnDb,
			string databaseInfoName)
		{
			var status = new StatusGenericHandler
			{ Message = $"Successfully moved the tenant to the database defined by the database information with the name '{databaseInfoName}'." };

			if (!_tenantType.IsSharding())
				throw new AuthPermissionsException(
					"This method can only be called when sharding is turned on.");

			var tenantChangeService = _tenantChangeServiceFactory.GetService();

			await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
			try
			{
				var tenant = await _context.Tenants
					.SingleOrDefaultAsync(x => x.TenantId == tenantToMoveId);

				if (tenant == null)
					return status.AddError("Could not find the tenant you were looking for.");

				if (tenant.DatabaseInfoName == databaseInfoName)
				{
					if (tenant.HasOwnDb == hasOwnDb)
						return status.AddError("You didn't change any of the sharding parts, so nothing was changed.");

					status.Message = $"The tenant wasn't moved but its {nameof(Tenant.HasOwnDb)} was changed to {hasOwnDb}.";
				}

				if (status.CombineStatuses(await CheckHasOwnDbIsValidAsync(hasOwnDb, databaseInfoName)).HasErrors)
					return status;

				var previousDatabaseInfoName = tenant.DatabaseInfoName;
				var previousDataKey = tenant.GetTenantDataKey();
				tenant.UpdateShardingState(databaseInfoName, hasOwnDb);

				if (status.CombineStatuses(await _context.SaveChangesWithChecksAsync()).HasErrors)
					return status;

				if (previousDatabaseInfoName != databaseInfoName)
				{
					//Just changes the HasNoDb part
					var mainError = await tenantChangeService
						.MoveToDifferentDatabaseAsync(previousDatabaseInfoName, previousDataKey, tenant);
					if (mainError != null)
						return status.AddError(mainError);
				}

				if (status.IsValid)
					await transaction.CommitAsync();
			}
			catch (Exception e)
			{
				if (_logger == null)
					throw;

				_logger.LogError(e, $"Failed to {e.Message}");
				return status.AddError(
					"The attempt to move the tenant to another database failed. Please contact the admin team.");
			}

			return status;
		}

		//----------------------------------------------------------
		// private methods

		/// <summary>
		/// If the hasOwnDb is true, it returns an error if any tenants have the same <see cref="Tenant.DatabaseInfoName"/>
		/// </summary>
		/// <param name="hasOwnDb"></param>
		/// <param name="databaseInfoName"></param>
		/// <returns>status</returns>
		private async Task<IStatusGeneric> CheckHasOwnDbIsValidAsync(bool hasOwnDb, string databaseInfoName)
		{
			var status = new StatusGenericHandler();
			if (!hasOwnDb)
				return status;

			databaseInfoName ??= _options.ShardingDefaultDatabaseInfoName;

			if (await _context.Tenants.AnyAsync(x => x.DatabaseInfoName == databaseInfoName))
				status.AddError(
					$"The {nameof(hasOwnDb)} parameter is true, but the sharding database name " +
					$"'{databaseInfoName}' already has tenant(s) using that database.");

			return status;
		}

		/// <summary>
		/// This finds the roles with the given names from the AuthP database. Returns errors if not found
		/// NOTE: The Tenant checks that the role's <see cref="RoleToPermissions.RoleType"/> are valid for a tenant
		/// </summary>
		/// <param name="tenantRoleNames">List of role name. Can be null, which means no roles to add</param>
		/// <returns>Status</returns>
		private async Task<IStatusGeneric<List<RoleToPermissions>>> GetRolesWithChecksAsync(
			List<string> tenantRoleNames)
		{
			var status = new StatusGenericHandler<List<RoleToPermissions>>();

			var foundRoles = tenantRoleNames?.Any() == true
				? await _context.RoleToPermissions
					.Where(x => tenantRoleNames.Contains(x.RoleName))
					.Distinct()
					.ToListAsync()
				: new List<RoleToPermissions>();

			if (foundRoles.Count != (tenantRoleNames?.Count ?? 0))
			{
				foreach (var badRoleName in tenantRoleNames.Where(x => !foundRoles.Select(y => y.RoleName).Contains(x)))
				{
					status.AddError($"The Role '{badRoleName}' was not found in the lists of Roles.");
				}
			}

			return status.SetResult(foundRoles);
		}
	}
}