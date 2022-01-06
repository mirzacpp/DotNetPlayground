using Microsoft.Extensions.FileProviders;
using System.Diagnostics.CodeAnalysis;

namespace Studens.Extensions.FileProviders.Amazon;

/// <summary>
/// Represents amazon file info
/// </summary>
public class AmazonFileInfo : IFileInfo
{
    public bool Exists => true;

    public long Length => -1;

    public string PhysicalPath => throw new NotImplementedException();

    public string Name => throw new NotImplementedException();

    public DateTimeOffset LastModified => throw new NotImplementedException();

    public bool IsDirectory => throw new NotImplementedException();

    [DoesNotReturn]
    public Stream CreateReadStream()
    {
        throw new NotSupportedException("File content cannot be opened as a stream.");
    }
}