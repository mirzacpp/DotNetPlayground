using Microsoft.Extensions.FileProviders;
using System.Diagnostics.CodeAnalysis;

namespace Studens.Extensions.FileProviders.Amazon;

/// <summary>
/// Represents amazon file info
/// </summary>
public class AmazonFileInfo : IFileInfo
{
    public AmazonFileInfo(string path)
    {
        PhysicalPath = path;
        Name = Path.GetFileName(path);
    }

    public bool Exists => true;

    public long Length => -1;

    /// <summary>
    /// Object URL
    /// </summary>
    public string PhysicalPath { get; init; }

    public string Name { get; init; }

    public DateTimeOffset LastModified => DateTime.MinValue;

    public bool IsDirectory => false;

    [DoesNotReturn]
    public Stream CreateReadStream()
    {
        throw new NotSupportedException("File content cannot be opened as a stream.");
    }
}