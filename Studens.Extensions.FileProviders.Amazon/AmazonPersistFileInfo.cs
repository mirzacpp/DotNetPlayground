using Amazon.S3;

namespace Studens.Extensions.FileProviders.Amazon;

/// <summary>
/// Represents amazon persistance file info
/// </summary>
public class AmazonPersistFileInfo : PersistFileInfo
{
    public AmazonPersistFileInfo(byte[] content, string name, string path, bool overwriteExisting, S3CannedACL cannedACL)
        : base(content, name, path, overwriteExisting)
    {
        CannedACL = cannedACL;
    }

    /// <summary>
    /// Gets or sets the canned access control list (ACL) for the uploaded object. Please
    /// refer to Amazon.S3.S3CannedACL for information on Amazon S3 canned ACLs.
    /// </summary>
    public S3CannedACL CannedACL { get; set; }
}