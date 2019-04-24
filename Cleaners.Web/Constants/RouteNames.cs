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
    }

    public static class UserRoutes
    {
        public const string Index = "User.Index";
        public const string Data = "User.Data";
        public const string Create = "User.Create";
        public const string Update = "User.Update";
        public const string ConfirmEmail = "User.ConfirmEmail";
    }
}