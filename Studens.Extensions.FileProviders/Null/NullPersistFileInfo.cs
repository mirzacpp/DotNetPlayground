using System.Diagnostics.CodeAnalysis;

namespace Studens.Extensions.FileProviders.Null;

/// <summary>
/// Null persist file info
/// </summary>
public class NullPersistFileInfo : PersistFileInfoBase
{
    public NullPersistFileInfo()
        : base(Array.Empty<byte>(), string.Empty, string.Empty, false)
    {
    }

    [DoesNotReturn]
    public override Stream CreateReadStream() =>
        throw new NotSupportedException("Null persist file info cannot be read.");
}