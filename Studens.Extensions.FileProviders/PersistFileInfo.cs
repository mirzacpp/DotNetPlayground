namespace Studens.Extensions.FileProviders;

/// <summary>
/// Represents file info used for persistance
/// </summary>
public class PersistFileInfo : PersistFileInfoBase
{
    public PersistFileInfo(byte[] content, string name, string subpath, bool overwriteExisting)
        : base(content, name, subpath, overwriteExisting)
    {
    }
}