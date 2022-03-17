namespace Studens.Extensions.FileProviders.Null
{
    public class NullFile : IFile
    {
        public string Name => throw new NotImplementedException();

        public string Path => throw new NotImplementedException();

        public DateTimeOffset LastModified => throw new NotImplementedException();
    }
}