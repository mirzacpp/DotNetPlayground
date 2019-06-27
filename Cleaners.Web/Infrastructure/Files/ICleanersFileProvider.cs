using Microsoft.Extensions.FileProviders;

namespace Cleaners.Web.Infrastructure.Files
{
    /// <summary>
    /// File provider service that can be used for content outside application directory
    /// </summary>
    public interface ICleanersFileProvider
    {
        IFileProvider FileProvider { get; }

        string BasePath { get; }

        void CreateDirectory(string path);

        /// <summary>
        /// Converts relative path to absolute path
        /// </summary>
        /// <param name="relativePathSegments"></param>
        /// <returns></returns>
        string MapPath(string relativePath);

        bool IsRootPath(string path);
    }
}