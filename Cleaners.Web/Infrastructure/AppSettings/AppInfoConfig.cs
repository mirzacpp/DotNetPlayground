namespace Cleaners.Web.Infrastructure.AppSettings
{
    /// <summary>
    /// Holds basic informations about application
    /// </summary>
    /// <remarks>
    /// Config registered as IOptionSnapshot so it can be configured without recompilation
    /// </remarks>
    public class AppInfoConfig
    {
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string Version { get; set; }
        public string TitleDelimiter { get; set; }
        public string Developer { get; set; }
        public string DevelopersSite { get; set; }
    }
}