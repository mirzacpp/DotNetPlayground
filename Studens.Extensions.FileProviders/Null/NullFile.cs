namespace Studens.Extensions.FileProviders.Null
{
    public class NullFile : IFile
    {
        public string Name => string.Empty;
        public string Path => string.Empty;
        public DateTimeOffset LastModified => DateTimeOffset.MinValue;

        public long Length => throw new NotImplementedException();
    }
}