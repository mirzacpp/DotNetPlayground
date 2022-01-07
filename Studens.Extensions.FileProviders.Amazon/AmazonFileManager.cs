using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Studens.Extensions.FileProviders.Amazon;

/// <summary>
/// Amazon file manager implementation
/// </summary>
/// <remarks>
/// <see cref="https://docs.aws.amazon.com/AmazonS3/latest/userguide/ShareObjectPreSignedURL.html"/>
/// </remarks>
public class AmazonFileManager : IFileManager
{
    private readonly AmazonFileManagerOptions _options;
    private readonly IAmazonS3 _amazonS3;
    private readonly ILogger<AmazonFileManager> _logger;
    private readonly FileProviderErrorDescriber _errorDescriber;

    public AmazonFileManager(IOptions<AmazonFileManagerOptions> optionsAccessor,
        IAmazonS3 amazonS3,
        ILogger<AmazonFileManager> logger,
        FileProviderErrorDescriber errorDescriber)
    {
        _options = optionsAccessor.Value;
        _amazonS3 = amazonS3;
        _logger = logger;
        _errorDescriber = errorDescriber;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="fileInfo"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <remarks>
    /// See <see cref="https://docs.aws.amazon.com/AmazonS3/latest/userguide/mpu-upload-object.html"/>
    /// TODO: There are some useful stuff in <see cref="TransferUtilityUploadRequest"/> like tags, metadata etc.
    /// </remarks>
    public async Task<FileResult> SaveAsync(PersistFileInfo fileInfo, CancellationToken cancellationToken = default)
    {
        try
        {
            var fullPath = GetFullPath(fileInfo.Path);
            var transferUtility = new TransferUtility(_amazonS3);
            using var stream = fileInfo.CreateReadStream();

            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = stream,
                Key = fullPath,
                BucketName = _options.BucketName,
                CannedACL = S3CannedACL.PublicRead
            };

            await transferUtility.UploadAsync(uploadRequest, cancellationToken);

            // Create new file info and return
            return FileResult.FileCreatedResult(new AmazonFileInfo(fullPath));
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError("An error occured while uploading object to server.", ex);
            return new FileResult(_errorDescriber.DefaultError(), ex);
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occured while uploading object to server.", ex);
            return new FileResult(_errorDescriber.DefaultError(), ex);
        }
    }

    public async Task<FileResult> DeleteAsync(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            return FileResult.FileNotFoundResult(filePath);
        }

        var fullPath = GetFullPath(filePath);

        try
        {
            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = _options.BucketName,
                Key = fullPath
            };

            await _amazonS3.DeleteObjectAsync(deleteRequest);

            return FileResult.FileDeleteResult();
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError("An error occured while uploading object to server.", ex);
            return new FileResult(_errorDescriber.DefaultError(), ex);
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occured while uploading object to server.", ex);
            return new FileResult(_errorDescriber.DefaultError(), ex);
        }
    }

    public Task<IEnumerable<string>> EnumerateFilesAsync(string directoryPath, string searchPattern, bool topDirectoryOnly = true)
    {
        throw new NotImplementedException();
    }

    public IDirectoryContents GetDirectoryContents(string subpath)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    /// <remarks>
    ///
    /// </remarks>
    public IFileInfo GetFileInfo(string subpath)
    {
        //var fullPath = GetFullPath(subpath);

        //var request = new GetObjectMetadataRequest
        //{
        //    BucketName = _options.BucketName,
        //    Key = fullPath
        //};

        //using var response = _amazonS3.GetObjectMetadataAsync(request);

        return null;
    }

    /// <inheritdoc/>
    public IChangeToken Watch(string filter) => NullChangeToken.Singleton;

    /// <summary>
    /// Combines amazon root path directory with given subpath
    /// </summary>
    /// <param name="path">Subpath</param>
    /// <returns>Absolute path</returns>
    private string GetFullPath(string path) => Path.Combine(_options.RootPath, path);
}