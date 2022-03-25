using Microsoft.Extensions.FileProviders;

namespace Studens.Extensions.FileProviders
{
    /// <summary>
    /// Represents file managing abstractions.
    /// </summary>
    /// <typeparam name="TPersistFileInfo">Type of the persistance file</typeparam>
    /// <typeparam name="TFileInfo">Type of the return file info</typeparam>
    public interface IFileManager<TFileInfo, TPersistFileInfo>
        where TPersistFileInfo : PersistFileInfoBase
        where TFileInfo : IFile
    {
        /// <summary>
        /// Persist file to selected location and then returns it.
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Operation result</returns>
        Task<FileResult<TFileInfo>> SaveAsync(TPersistFileInfo fileInfo, CancellationToken cancellationToken = default);

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
        ValueTask<TFileInfo> GetFileInfoAsync(string path);        
    }
}