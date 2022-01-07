using Microsoft.Extensions.FileProviders;
using System.Diagnostics.CodeAnalysis;

namespace Studens.Extensions.FileProviders.Amazon;

/// <summary>
/// Represents amazon file info
/// </summary>
public class AmazonFileInfo : IFileInfo
{
    public AmazonFileInfo(string path, DateTimeOffset lastModified, long length)
    {
        PhysicalPath = path;
        LastModified = lastModified;
        Length = length;
        Name = Path.GetFileName(path);
    }

    public bool Exists => true;

    public long Length { get; init; }

    /// <summary>
    /// Object URL
    /// </summary>
    public string PhysicalPath { get; init; }

    public string Name { get; init; }

    public DateTimeOffset LastModified { get; init; }

    public bool IsDirectory => false;

    [DoesNotReturn]
    public Stream CreateReadStream()
    {
        throw new NotSupportedException("File content cannot be opened as a stream.");
    }
}