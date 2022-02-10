using Microsoft.Extensions.FileProviders;

namespace Studens.Extensions.FileProviders.Null;

/// <summary>
/// File provider that does nothing.
/// </summary>
public class NullFileProvider : FileManagerBase<NotFoundFileInfo, NullPersistFileInfo>
{
    public static NullFileProvider Instance = new();

    public override Task<FileResult> DeleteAsync(string filePath) =>
        Task.FromResult(new FileResult(FileOperationStatus.Deleted));

    public override Task<IEnumerable<string>> EnumerateFilesAsync(string path, string searchPattern, bool topDirectoryOnly = true) =>
        Task.FromResult(Enumerable.Empty<string>());

    public override ValueTask<NotFoundFileInfo> GetFileInfoAsync(string path)
        => ValueTask.FromResult(new NotFoundFileInfo(path));

    public override Task<FileResult<NotFoundFileInfo>> SaveAsync(NullPersistFileInfo fileInfo, CancellationToken cancellationToken = default) =>
        Task.FromResult(new FileResult<NotFoundFileInfo>(FileOperationStatus.Created, new NotFoundFileInfo(string.Empty)));
}