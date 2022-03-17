namespace Studens.Extensions.FileProviders;

/// <summary>
/// Base implementation for file management.
/// </summary>
public abstract class FileManagerBase<TFileInfo, TPersistFileInfo>
    : IFileManager<TFileInfo, TPersistFileInfo>
    where TPersistFileInfo : PersistFileInfoBase
    where TFileInfo : IFile
{
    public abstract Task<FileResult<TFileInfo>> SaveAsync(TPersistFileInfo fileInfo, CancellationToken cancellationToken = default);

    public abstract Task<FileResult> DeleteAsync(string filePath);

    public abstract Task<IEnumerable<string>> EnumerateFilesAsync(string path, string searchPattern, bool topDirectoryOnly = true);

    public abstract ValueTask<TFileInfo> GetFileInfoAsync(string path);
}