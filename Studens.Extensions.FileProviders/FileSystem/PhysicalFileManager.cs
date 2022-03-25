using Ardalis.GuardClauses;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

namespace Studens.Extensions.FileProviders.FileSystem;

/// <summary>
/// File system implementation of <see cref="IFileManager"/>
/// </summary>
/// <remarks>
/// <see cref="https://github.com/dotnet/runtime/blob/main/src/libraries/Microsoft.Extensions.FileProviders.Physical/src/PhysicalFileProvider.cs"/>
/// <see cref="https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/IO/File.cs"/>
/// <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.io.file?view=net-6.0"/>
/// <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.io.directory?view=net-6.0"/>
/// </remarks>
public class PhysicalFileManager : FileManagerBase<PhysicalFile, PersistFileInfo>
{
    #region Fields

    private static readonly char[] _pathSeparators = new[]
            {Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar};

    private string Root { get; init; }

    private readonly FileProviderErrorDescriber _errorDescriber;
    private readonly FileIOExecutor _fileIOExecutor;

    /// <summary>
    /// File provider used for built-in file provider.
    /// </summary>
    private readonly IFileProvider _fileProvider;

    #endregion Fields

    #region Ctor

    public PhysicalFileManager(IOptions<PhysicalFileManagerOptions> optionsAccessor,
        FileProviderErrorDescriber errorDescriber,
        FileIOExecutor fileIOExecutor)
    {
        _fileProvider = new PhysicalFileProvider(optionsAccessor.Value.RootPath);
        Root = optionsAccessor.Value.RootPath;
        _errorDescriber = errorDescriber ?? new FileProviderErrorDescriber();
        _fileIOExecutor = fileIOExecutor;
    }

    #endregion Ctor

    #region Methods

    /// <summary>
    /// Persist file to selected location and then returns it.
    /// </summary>
    /// <param name="fileInfo"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Operation result</returns>
    public override async Task<FileResult<PhysicalFile>> SaveAsync(PersistFileInfo fileInfo, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(fileInfo, nameof(fileInfo));

        // Relative paths starting with leading slashes are okay
        var path = fileInfo.Subpath.TrimStart(_pathSeparators);

        // Absolute paths not permitted.
        if (Path.IsPathRooted(path))
        {
            return FileResult<PhysicalFile>.ErrorResult(_errorDescriber.InvalidPath());

            //return new FileResult<PhysicalFile>(_errorDescriber.InvalidPath());
        }

        var fullPath = GetFullPath(path);

        if (string.IsNullOrEmpty(fullPath))
        {
            return new FileResult(_errorDescriber.InvalidPath());
        }

        EnsureDirectoryExists(fullPath);
        var relativeFileName = Path.Combine(path, fileInfo.Name);
        var existingFileInfo = GetFileInfoInternal(relativeFileName);

        if (!fileInfo.OverwriteExisting && existingFileInfo.Exists && !existingFileInfo.IsDirectory)
        {
            return FileResult.FileUnmodifiedResult(new PhysicalFile(existingFileInfo));
        }

        var fullFileName = Path.Combine(fullPath, fileInfo.Name);

        using var stream = fileInfo.CreateReadStream();
        var bytes = await stream.GetAllBytesAsync(cancellationToken);

        Func<ValueTask<FileResult>> successResult = existingFileInfo.Exists && !existingFileInfo.IsDirectory ?
            async () => FileResult.FileModifiedResult(await GetFileInfoAsync(relativeFileName)) :
            async () => FileResult.FileCreatedResult(await GetFileInfoAsync(relativeFileName));

        return await _fileIOExecutor.TryExecuteAsync(
            ioAction: () => File.WriteAllBytesAsync(fullFileName, bytes, cancellationToken),
            successResult);
    }

    /// <summary>
    /// Deletes file at given path.
    /// </summary>
    /// <param name="filePath">File path</param>
    /// <returns>Operation result</returns>
    public override Task<FileResult> DeleteAsync(string filePath)
    {
        var fullFileName = GetFullPath(filePath);

        if (string.IsNullOrEmpty(fullFileName))
        {
            return Task.FromResult(FileResult.FileNotFoundResult(filePath));
        }

        return Task.FromResult(_fileIOExecutor.TryExecute(
            ioAction: () => File.Delete(fullFileName),
            successResult: () => FileResult.FileDeleteResult()));
    }

    /// <summary>
    /// Enumerates files in directory at given path.
    /// </summary>
    /// <param name="path">Directory path</param>
    /// <param name="searchPattern">Search pattern</param>
    /// <param name="topDirectoryOnly">Indicates if only top directory should be enumerated</param>
    /// <returns>Files</returns>
    public override Task<IEnumerable<string>> EnumerateFilesAsync(string path, string searchPattern, bool topDirectoryOnly = true)
    {
        // Return FileOptions ?
        var fullPath = GetFullPath(path);

        // If path null, return empty collection

        return Task.FromResult(Directory.EnumerateFiles(
            fullPath,
            searchPattern,
            topDirectoryOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories));
    }

    public override ValueTask<PhysicalFile> GetFileInfoAsync(string filePath)
    {
        return ValueTask.FromResult(_fileProvider.GetFileInfo(filePath));
    }

    public ValueTask<IDirectoryContents> GetDirectoryContentsAsync(string path)
    {
        return ValueTask.FromResult(_fileProvider.GetDirectoryContents(path));
    }

    #endregion Methods

    #region Utils

    /// <summary>
    /// TODO: Update return type to FileResult and use executor?
    /// </summary>
    /// <param name="path">Directory path</param>
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

    private IFileInfo GetFileInfoInternal(string filePath)
    {
        return _fileProvider.GetFileInfo(filePath);
    }

    #endregion Utils
}