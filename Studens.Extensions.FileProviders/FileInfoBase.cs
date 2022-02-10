using Microsoft.Extensions.FileProviders;

namespace Studens.Extensions.FileProviders
{
    public abstract class FileInfoBase : IFileInfo
    {
        public virtual bool Exists { get; }

        public long Length => throw new NotImplementedException();

        public string PhysicalPath => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public DateTimeOffset LastModified => throw new NotImplementedException();

        public bool IsDirectory => throw new NotImplementedException();

        public abstract Stream CreateReadStream();
    }
}