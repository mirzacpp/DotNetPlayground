namespace Studens.MultitenantApp.Web.Data
{
	public static class ApplicationRoles
	{
		/// <summary>
		/// Global access
		/// </summary>
		public const string SuperAdmin = nameof(SuperAdmin);

		/// <summary>
		/// Global access
		/// </summary>
		public const string Admin = nameof(Admin);

		/// <summary>
		/// Manages team members
		/// </summary>
		public const string HumanResources = nameof(HumanResources);

		/// <summary>
		/// Represents team member
		/// </summary>
		public const string TeamMember = nameof(TeamMember);

		/// <summary>
		/// Manages news
		/// </summary>
		public const string ContentEditor = nameof(ContentEditor);

		/// <summary>
		/// Allows access to public API resources
		/// </summary>
		public const string Guest = nameof(Guest);

		public static class Rules
		{
			public static string DefaultRole => TeamMember;

			/// <summary>
			/// Determines if specified <paramref name="roleName"/> matches configured default role.
			/// </summary>
			/// <param name="roleName">Role to check</param>
			public static bool IsDefaultRole(string roleName) =>
			  roleName.Equals(DefaultRole, StringComparison.OrdinalIgnoreCase);

			public static bool RequiresTeamMemberFullWritePolicy(string roleName) =>
				   new[] { HumanResources, Admin, ContentEditor }.Contains(roleName);

			public static bool HasAccessToBackOffice(IEnumerable<string> roles) =>
			  roles.Any(r => new[] { HumanResources, Admin, ContentEditor }.Contains(r));
		}
	}
}