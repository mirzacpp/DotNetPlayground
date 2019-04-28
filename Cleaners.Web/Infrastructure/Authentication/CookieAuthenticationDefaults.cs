namespace Cleaners.Web.Infrastructure.Authentication
{
    /// <summary>
    /// Cookie authentication configuration
    /// </summary>
    public static class CookieAuthenticationDefaults
    {
        public const bool HttpOnly = true;
        public const bool SlidingExpiration = true;
        public const string AccessDeniedPath = "/Account/AccessDenied";
        public const string LoginPath = "/Account/Login";
        public const string LogoutPath = "/Account/Logout";
    }
}