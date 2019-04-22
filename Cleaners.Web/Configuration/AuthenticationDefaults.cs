namespace Cleaners.Web.Configuration
{
    /// <summary>
    /// Authentication configuration
    /// </summary>
    public static class AuthenticationDefaults
    {
        public const bool LockoutEnabled = true;
        public const bool RequireDigit = true;
        public const bool RequireLowercase = true;
        public const bool RequireNonAlphanumeric = true;
        public const bool RequireUppercase = true;
        public const int RequiredLength = 6;
        public const int RequiredUniqueChars = 3;
        public const bool RequireConfirmedEmail = true;
        public const bool RequireConfirmedPhoneNumber = false;
        public const bool HttpOnly = true;
        public const bool SlidingExpiration = true;
        public const string AccessDeniedPath = "/Accounts/AccessDenied";
        public const string LoginPath = "/Accounts/Login";
        public const string LogoutPath = "/Accounts/Logout";
    }
}