using Microsoft.Extensions.FileProviders;

namespace Studens.Extensions.FileProviders
{
    /// <summary>
    /// Extends existing radonly only <see cref="IFileProvider"/> with write operations.
    /// </summary>
    public interface IFileManager : IFileProvider
    {
        /// <summary>
        /// Persist file to selected location and then returns it.
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<FileResult> SaveAsync(PersistFileInfo fileInfo, CancellationToken cancellationToken = default);

        Task<FileResult> DeleteAsync(string filePath);
    }
}