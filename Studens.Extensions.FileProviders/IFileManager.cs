using Microsoft.Extensions.FileProviders;

namespace Studens.Extensions.FileProviders
{
    /// <summary>
    /// Extends existing radonly only <see cref="IFileProvider"/> with write operations.
    /// </summary>
    public interface IFileManager : IFileProvider
    {
        Task<string> SaveAsync(PersistFileInfo fileInfo, CancellationToken cancellationToken = default);
    }
}