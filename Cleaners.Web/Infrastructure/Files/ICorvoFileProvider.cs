using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace Cleaners.Web.Infrastructure.Files
{
    /// <summary>
    /// Physical file provider
    /// This provider can be used if we don't want to store files inside application directory
    /// For inside application files use default <see cref="IHostingEnvironment.ContentRootFileProvider"/>
    /// </summary>
    public interface ICorvoFileProvider
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