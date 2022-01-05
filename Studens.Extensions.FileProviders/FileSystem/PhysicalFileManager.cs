using Ardalis.GuardClauses;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Studens.Commons.Extensions;

namespace Studens.Extensions.FileProviders.FileSystem;

public class PhysicalFileManager : PhysicalFileProvider, IFileManager
{
    #region Fields

    private readonly ILogger<PhysicalFileManager> _logger;
    private readonly FileProviderErrorDescriber _errorDescriber;

    #endregion Fields

    #region Ctor

    public PhysicalFileManager(IOptions<PhysicalFileManagerOptions> optionsAccessor,
        ILogger<PhysicalFileManager> logger,
        FileProviderErrorDescriber? errorDescriber)
        : base(optionsAccessor.Value.Path)
    {
        _logger = logger;
        _errorDescriber = errorDescriber ?? new FileProviderErrorDescriber();
    }

    #endregion Ctor

    #region Methods

    public async Task<FileResult> SaveAsync(PersistFileInfo fileInfo, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(fileInfo, nameof(fileInfo));

        var path = fileInfo.Path;

        var fullPath = GetFullPath(path);

        if (string.IsNullOrEmpty(fullPath))
        {
            return new FileResult(_errorDescriber.InvalidPath());            
        }

        EnsureDirectoryExists(fullPath);
        var relativeFileName = Path.Combine(path, fileInfo.Name);
        IFileInfo existingFileInfo = GetFileInfo(relativeFileName);

        if (!fileInfo.OverwriteExisting && existingFileInfo.Exists && !existingFileInfo.IsDirectory)
        {
            return FileResult.FileUnmodifiedResult(existingFileInfo);
        }

        var fullFileName = Path.Combine(fullPath, fileInfo.Name);

        using var stream = fileInfo.CreateReadStream();
        var bytes = await stream.GetAllBytesAsync(cancellationToken);
        await File.WriteAllBytesAsync(fullFileName, bytes, cancellationToken);

        return existingFileInfo.Exists && !existingFileInfo.IsDirectory ?
            FileResult.FileModifiedResult(GetFileInfo(relativeFileName)) :
            FileResult.FileCreatedResult(GetFileInfo(relativeFileName));
    }

    /// <summary>
    /// TODO: Trim starting trail char ?
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public Task<FileResult> DeleteAsync(string filePath)
    {
        // Avoid throwing ? yes, File.Delete does not throw if file not found
        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentException($"'{nameof(filePath)}' cannot be null or empty.", nameof(filePath));
        }

        var fullFileName = GetFullPath(filePath);

        if (string.IsNullOrEmpty(fullFileName))
        {
            throw new ArgumentException($"'{nameof(filePath)}' cannot be null or empty.", nameof(filePath));
        }

        File.Delete(fullFileName);

        return Task.FromResult(FileResult.FileDeleteResult());
    }

    #endregion Methods

    #region Utils

    private void EnsureDirectoryExists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    /// <summary>
    /// This is a private method from <see cref="PhysicalFileProvider"/>
    /// </summary>
    private string? GetFullPath(string path)
    {
        if (PathUtils.PathNavigatesAboveRoot(path))
        {
            return null;
        }

        string fullPath;
        try
        {
            fullPath = Path.GetFullPath(Path.Combine(Root, path));
        }
        catch
        {
            return null;
        }

        if (!IsUnderneathRoot(fullPath))
        {
            return null;
        }

        return fullPath;
    }

    private bool IsUnderneathRoot(string fullPath) => fullPath.StartsWith(Root, StringComparison.OrdinalIgnoreCase);

    #endregion Utils
}