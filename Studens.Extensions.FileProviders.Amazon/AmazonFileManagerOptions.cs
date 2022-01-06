namespace Studens.Extensions.FileProviders.Amazon;

public class AmazonFileManagerOptions
{
    public string BucketName { get; set; }

    [Obsolete("Use root path and introduce base FileManagerOptions type?")]
    public string DomainPrefix { get; set; }
}