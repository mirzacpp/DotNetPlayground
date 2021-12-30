using Microsoft.Extensions.FileProviders;

namespace Studens.Extensions.FileProviders
{
    /// <summary>
    /// Extends existing radonly only <see cref="IFileProvider"/> with write operations.
    /// </summary>
    public interface IFileManager : IFileProvider
    {
        /// <summary>
        /// Persist file to selected location
        /// TODO: Return result object to avoid global exception handler ?
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //Task SaveAsync(PersistFileInfo fileInfo, CancellationToken cancellationToken = default);

        /// <summary>
        /// Persist file to selected location and then returns it.
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IFileInfo> SaveAsync(PersistFileInfo fileInfo, CancellationToken cancellationToken = default);
    }
}