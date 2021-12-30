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

    #endregion Fields

    #region Ctor

    public PhysicalFileManager(IOptions<PhysicalFileManagerOptions> optionsAccessor, ILogger<PhysicalFileManager> logger)
        : base(optionsAccessor.Value.Path)
    {
        _logger = logger;
    }

    #endregion Ctor

    #region Methods

    public async Task<IFileInfo> SaveAsync(PersistFileInfo fileInfo, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(fileInfo, nameof(fileInfo));

        var fullPath = GetFullPath(fileInfo.Subpath);

        if (string.IsNullOrEmpty(fullPath))
        {
            throw new InvalidOperationException("Directory could not be found.");
        }

        EnsureDirectoryExists(fullPath);

        var fullFileName = Path.Combine(fullPath, fileInfo.Name);

        using var stream = fileInfo.CreateReadStream();
        var bytes = await stream.GetAllBytesAsync(cancellationToken);
        await File.WriteAllBytesAsync(fullFileName, bytes, cancellationToken);

        return GetFileInfo(Path.Combine(fileInfo.Subpath, fileInfo.Name));
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
