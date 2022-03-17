using Microsoft.Extensions.FileProviders;

namespace Studens.Extensions.FileProviders.Null;

/// <summary>
/// File provider that does nothing.
/// </summary>
public class NullFileProvider : FileManagerBase<NullFile, NullPersistFileInfo>
{
    public static NullFileProvider Instance = new();

    public override Task<FileResult> DeleteAsync(string filePath) =>
        Task.FromResult(new FileResult(FileOperationStatus.Deleted));

    public override Task<IEnumerable<string>> EnumerateFilesAsync(string path, string searchPattern, bool topDirectoryOnly = true) =>
        Task.FromResult(Enumerable.Empty<string>());

    public override ValueTask<NullFile> GetFileInfoAsync(string path)
        => ValueTask.FromResult(new NullFile());

    public override Task<FileResult<NullFile>> SaveAsync(NullPersistFileInfo fileInfo, CancellationToken cancellationToken = default) =>
        Task.FromResult(new FileResult<NullFile>(FileOperationStatus.Created, new NotFoundFileInfo(string.Empty)));
}