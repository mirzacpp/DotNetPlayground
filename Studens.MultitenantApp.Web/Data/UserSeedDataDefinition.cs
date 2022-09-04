using Rev.AuthPermissions.BaseCode.DataLayer.Classes.SupportTypes;
using Rev.AuthPermissions.BaseCode.SetupCode;
using Studens.Commons.Extensions;

namespace Studens.MultitenantApp.Web.Data
{
	/// <summary>
	/// Holds data used for database startup seed.
	/// </summary>
	public static class UserSeedDataDefinition
	{
		/// <summary>
		/// Defines application roles
		/// </summary>
		public static readonly List<BulkLoadRolesDto> RolesDefinition = new()
		  {
			new(ApplicationRoles.SuperAdmin, "Super admin - only use for setup", "AccessAll"),
			new(ApplicationRoles.Admin, "Overall app Admin", "TeamMemberRead, TeamMemberCreate, TeamMemberUpdate, TeamMemberDelete"),
			new(ApplicationRoles.ContentEditor,
			"Manages application content", "NewsRead, NewsCreate, NewsUpdate, NewsDelete",
			RoleTypes.TenantAdminAdd),
			new(ApplicationRoles.HumanResources,
			"Manages application content", "TeamMemberRead, TeamMemberCreate, TeamMemberUpdate, TeamMemberDelete",
			RoleTypes.TenantAdminAdd),
		  };

		public static readonly List<BulkLoadUserWithRolesTenant> UsersRolesDefinition = new()
		  {
			  new ("superadmin@app.ba", null, "SuperAdmin"),
			  new ("app-admin@app.ba", null, "Admin"),
			  new ("app-admin-starmo@ito.app", null, "Admin", tenantNameForDataKey: "StarMo")
		  };

		public static readonly List<BulkLoadTenantDto> TenantDefinition = new()
		  {
			  new ("ITOdjel"),
			  new ("StarMo", new[] { ApplicationRoles.ContentEditor, ApplicationRoles.HumanResources}.JoinToString())
		  };
	}
}