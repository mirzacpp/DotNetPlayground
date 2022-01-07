namespace Studens.Extensions.FileProviders.Amazon;

public class AmazonFileManagerOptions
{
    public AmazonFileManagerOptions()
    {
        // We need to access region used for credentials
        PublicAccessUrlPrefix = $"https://s3.eu-central-1.amazonaws.com/test.ito.dev/test/";
    }

    /// <summary>
    /// Amazon bucket
    /// </summary>
    public string BucketName { get; set; }

    /// <summary>
    /// Remove and use bucket as a root directory.
    /// </summary>

    [Obsolete("Remove and use bucket as a root directory.")]
    public string RootPath { get; set; } = default!;

    /// <summary>
    /// This prop is used to build full URL to public object
    /// </summary>
    public string PublicAccessUrlPrefix { get; private set; }

    public string AccessKeyId { get; set; }
    public string SecretAccessKey { get; set; }
}