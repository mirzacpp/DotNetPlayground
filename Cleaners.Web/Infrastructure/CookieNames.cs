namespace Cleaners.Web.Infrastructure
{
    public static class CookieNames
    {
        public const string Prefix = ".Feal";
        public const string Authentication = Prefix + ".Auth";
        public const string TempData = Prefix + ".TempData";
        public const string AntiforgeryToken = Prefix + ".AntiforgeryToken";
        public const string Culture = Prefix + ".Culture";
    }
}