namespace Cleaners.Web.Constants
{
    public class RouteNames
    {
        public const string Default = "Default";
    }

    public static class HomeRoutes
    {
        public const string Index = "Home.Index";
    }

    public static class AccountRoutes
    {
        public const string Login = "Account.Login";
        public const string Logout = "Account.Logout";
        public const string ChangePassword = "Account.ChangePassword";
        public const string Profile = "Account.Profile";
    }

    public static class SettingsRoutes
    {
        public const string AuthenticationSettings = "Settings.AuthenticationSettings";
    }

    public static class UserRoutes
    {
        public const string Index = "User.Index";
        public const string Data = "User.Data";
        public const string Create = "User.Create";
        public const string Update = "User.Update";
        public const string Delete = "User.Delete";
        public const string Restore = "User.Restore";
        public const string ChangePassword = "User.ChangePassword";
        public const string ConfirmEmail = "User.ConfirmEmail";
        public const string Details = "User.Details";
    }

    public static class LocalizationRoutes
    {
        public const string ChangeLanguage = "Localization.ChangeLanguage";
    }

    public static class RoleRoutes
    {
        public const string Index = "Role.Index";
        public const string Data = "Role.Data";
        public const string Create = "Role.Create";
        public const string Update = "Role.Update";
        public const string Delete = "Role.Delete";
    }
}