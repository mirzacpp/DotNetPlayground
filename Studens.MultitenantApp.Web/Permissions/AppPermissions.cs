using System.ComponentModel.DataAnnotations;

namespace Studens.MultitenantApp.Web.Permissions
{
	public enum AppPermissions : ushort
	{
		NotSet = 0, //error condition

		[Display(GroupName = "TeamMembers", Name = "Read", Description = "Can see team members")]
		TeamMemberRead = 10,

		[Display(GroupName = "TeamMembers", Name = "Create", Description = "Can create team members")]
		TeamMemberCreate = 11,

		[Display(GroupName = "TeamMembers", Name = "Update", Description = "Can update team members")]
		TeamMemberUpdate = 12,

		[Display(GroupName = "TeamMembers", Name = "Delete", Description = "Can delete team members")]
		TeamMemberDelete = 13,

		[Display(GroupName = "NewsManagement", Name = "Read", Description = "Can see news")]
		NewsRead = 30,

		[Display(GroupName = "NewsManagement", Name = "Create", Description = "Can create news")]
		NewsCreate = 31,

		[Display(GroupName = "NewsManagement", Name = "Update", Description = "Can update news")]
		NewsUpdate = 32,

		[Display(GroupName = "NewsManagement", Name = "Delete", Description = "Can delete news")]
		NewsDelete = 33,

		//Setting the AutoGenerateFilter to true in the display allows we can exclude this permissions
		//to admin users who aren't allowed alter this permissions
		//Useful for multi-tenant applications where you can set up company-level admin users where you can hide some higher-level permissions
		[Display(GroupName = "SuperAdmin",
			Name = "AccessAll",
			Description = "This allows the user to access every feature",
			AutoGenerateFilter = true)]
		AccessAll = ushort.MaxValue,
	}
}