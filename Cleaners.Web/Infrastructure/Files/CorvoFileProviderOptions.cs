namespace Cleaners.Web.Infrastructure.Files
{
    /// <summary>
    /// Configuration options for file provider
    /// </summary>
    public class CorvoFileProviderOptions
    {
        /// <summary>
        /// Root/Base path ie. C:\\Cleaners
        /// </summary>
        /// <remarks>
        /// Add support for relative path ??
        /// If path is relative, just combine it with webrootpath ?
        /// </remarks>
        public string BasePath { get; set; }
    }
}