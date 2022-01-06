using Amazon.S3;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Studens.Extensions.FileProviders.Amazon;

/// <summary>
/// Amazon file manager implementation
/// </summary>
public class AmazonFileManager : IFileManager
{
    private readonly AmazonFileManagerOptions _options;
    private readonly IAmazonS3 _amazonS3;

    public AmazonFileManager(IOptions<AmazonFileManagerOptions> optionsAccessor, IAmazonS3 amazonS3)
    {
        _options = optionsAccessor.Value;
        _amazonS3 = amazonS3;
    }

    public Task<FileResult> SaveAsync(PersistFileInfo fileInfo, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<FileResult> DeleteAsync(string filePath)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<string>> EnumerateFilesAsync(string directoryPath, string searchPattern, bool topDirectoryOnly = true)
    {
        throw new NotImplementedException();
    }

    public IDirectoryContents GetDirectoryContents(string subpath)
    {
        throw new NotImplementedException();
    }

    public IFileInfo GetFileInfo(string subpath)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public IChangeToken Watch(string filter) => NullChangeToken.Singleton;

    /// <summary>
    /// Combines amazon root path directory with given subpath
    /// </summary>
    /// <param name="path">Subpath</param>
    /// <returns>Absolute path</returns>
    private string GetFullPath(string path) => Path.Combine(_options.DomainPrefix, path);
}