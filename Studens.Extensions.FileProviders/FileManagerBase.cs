using Microsoft.Extensions.FileProviders;

namespace Studens.Extensions.FileProviders
{
    /// <summary>
    /// Base implementation for file management.
    /// </summary>
    public abstract class FileManagerBase<TFileInfo, TPersistFileInfo>
        : IFileManager<TFileInfo, TPersistFileInfo>
        where TPersistFileInfo : IFileInfo
        where TFileInfo : IFileInfo
    {
        public abstract Task<FileResult> DeleteAsync(string filePath);

        public abstract Task<IEnumerable<string>> EnumerateFilesAsync(string path, string searchPattern, bool topDirectoryOnly = true);

        public abstract ValueTask<TFileInfo> GetFileInfoAsync(string path);

        public abstract Task<FileResult<TFileInfo>> SaveAsync(TPersistFileInfo fileInfo, CancellationToken cancellationToken = default);
    }
}