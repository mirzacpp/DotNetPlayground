using Amazon.Runtime.Internal;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;

namespace Studens.Extensions.FileProviders.Amazon;

/// <summary>
/// Amazon file manager implementation
/// </summary>
/// <remarks>
/// <see cref="https://docs.aws.amazon.com/AmazonS3/latest/userguide/ShareObjectPreSignedURL.html"/>
/// <see cref="https://stackoverflow.com/questions/9944671/amazon-s3-creating-folder-through-net-sdk-vs-through-management-console"/>
/// </remarks>
public class AmazonFileManager : IFileManager<AmazonPersistFileInfo>
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
    public async Task<FileResult> SaveAsync(AmazonPersistFileInfo fileInfo, CancellationToken cancellationToken = default)
    {
        try
        {
            var fullPath = PathUtils.Combine(fileInfo.Path, fileInfo.Name);
            var transferUtility = new TransferUtility(_amazonS3);
            using var stream = fileInfo.CreateReadStream();

            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = stream,
                Key = fullPath,
                BucketName = _options.BucketName,
                CannedACL = fileInfo.CannedACL,
            };            

            await transferUtility.UploadAsync(uploadRequest, cancellationToken);

            // Create new file info and return
            return FileResult.FileCreatedResult(new AmazonFileInfo(GetObjectUrl(fullPath, true), DateTimeOffset.UtcNow, fileInfo.Length));
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

        var fullPath = GetObjectUrl(filePath, true);

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

    /// <summary>
    /// TODO: Finish implementation
    /// <see cref="https://docs.aws.amazon.com/AmazonS3/latest/userguide/ListingKeysUsingAPIs.html"/>
    /// </summary>
    /// <param name="directoryPath"></param>
    /// <param name="searchPattern"></param>
    /// <param name="topDirectoryOnly"></param>
    /// <returns></returns>
    public async Task<IEnumerable<string>> EnumerateFilesAsync(string directoryPath, string searchPattern, bool topDirectoryOnly = true)
    {
        if (directoryPath.StartsWith('/'))
        {
            directoryPath = directoryPath.TrimStart('/');
        }

        try
        {
            var request = new ListObjectsV2Request
            {
                BucketName = _options.BucketName,
                //MaxKeys = 10,
                //Delimiter = "/",
                Prefix = "vlado/"
            };
            ListObjectsV2Response response;
            var objects = new List<string>();

            do
            {
                // Pass cancellation token
                response = await _amazonS3.ListObjectsV2Async(request);

                foreach (var @object in response.S3Objects)
                {
                    objects.Add(@object.Key);
                }

                request.ContinuationToken = response.NextContinuationToken;
            } while (response.IsTruncated);

            return objects;
        }
        catch (AmazonS3Exception amazonS3Exception)
        {
            Console.WriteLine("S3 error occurred. Exception: " + amazonS3Exception.ToString());
            return Enumerable.Empty<string>();
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.ToString());
            return Enumerable.Empty<string>();
        }
    }

    public IDirectoryContents GetDirectoryContents(string subpath)
    {
        throw new NotImplementedException();
    }

    ///<inheritdoc/>
    public async ValueTask<IFileInfo> GetFileInfoAsync(string path)
    {
        // Absolute paths not permitted.
        if (Path.IsPathRooted(path))
        {
            return new NotFoundFileInfo(path);
        }

        var request = new GetObjectMetadataRequest
        {
            BucketName = _options.BucketName,
            Key = path
        };

        try
        {
            var response = await _amazonS3.GetObjectMetadataAsync(request);

            if (response is null)
            {
                return new NotFoundFileInfo(path);
            }

            return new AmazonFileInfo(GetObjectUrl(path, true), response.LastModified, response.ContentLength);
        }
        catch (HttpErrorResponseException ex)
        {
            throw;
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            // Log
            return new NotFoundFileInfo(path);
        }
    }

    public ValueTask<IDirectoryContents> GetDirectoryContentsAsync(string path)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    //public IChangeToken Watch(string filter) => NullChangeToken.Singleton;

    /// <summary>
    /// Combines amazon root path directory with given subpath
    /// </summary>
    /// <param name="path">Subpath</param>
    /// <param name="isPublic">Indicates whether this object is allowed for public read.</param>
    /// <returns>Absolute path</returns>
    private string GetObjectUrl(string path, bool isPublic)
    {
        // We do not have to use s3 method for public reads.
        if (isPublic)
        {
            return _options.PublicAccessUrlPrefix + path;
        }

        var urlRequest = new GetPreSignedUrlRequest
        {
            BucketName = _options.BucketName,
            Key = path,
            Expires = DateTime.Now.AddMinutes(15), // Move to options
            Protocol = Protocol.HTTPS
        };

        return _amazonS3.GetPreSignedURL(urlRequest);
    }
}