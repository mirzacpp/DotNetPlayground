using Microsoft.Extensions.FileProviders;

namespace Studens.Extensions.FileProviders
{
    /// <summary>
    /// Extends existing radonly only <see cref="IFileProvider"/> with write operations.
    /// </summary>
    public interface IFileManager<TPersistFileInfo> where TPersistFileInfo : IFileInfo
    {
        /// <summary>
        /// Persist file to selected location and then returns it.
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Operation result</returns>
        Task<FileResult> SaveAsync(TPersistFileInfo fileInfo, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes file at given path.
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <returns>Operation result</returns>
        Task<FileResult> DeleteAsync(string filePath);

        /// <summary>
        /// Enumerates files in directory at given path.
        /// </summary>
        /// <param name="path">Directory path</param>
        /// <param name="searchPattern">Search pattern</param>
        /// <param name="topDirectoryOnly">Indicates if only top directory should be enumerated</param>
        /// <returns>Files</returns>
        Task<IEnumerable<string>> EnumerateFilesAsync(string path, string searchPattern, bool topDirectoryOnly = true);

        /// <summary>
        /// Returns file info for given path.
        /// </summary>
        /// <param name="path">Relative file path.</param>
        /// <returns>File info</returns>
        ValueTask<IFileInfo> GetFileInfoAsync(string path);

        /// <summary>
        /// Returns all files for a directory at given path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        ValueTask<IDirectoryContents> GetDirectoryContentsAsync(string path);
    }
}