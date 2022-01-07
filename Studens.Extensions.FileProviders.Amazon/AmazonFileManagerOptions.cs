namespace Studens.Extensions.FileProviders.Amazon;

public class AmazonFileManagerOptions
{
    /// <summary>
    /// Amazon bucket
    /// </summary>
    public string BucketName { get; set; } = default!;

    /// <summary>
    /// Root path to resolve other directories
    /// </summary>

    [Obsolete("Use root path and introduce base FileManagerOptions type?")]
    public string RootPath { get; set; } = default!;
}