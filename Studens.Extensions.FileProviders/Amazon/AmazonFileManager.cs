using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace Studens.Extensions.FileProviders.Amazon
{
    /// <summary>
    /// Amazon file manager implementation
    /// </summary>
    public class AmazonFileManager : IFileManager
    {
        public Task DeleteAsync(string filePath)
        {
            throw new NotImplementedException();
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            throw new NotImplementedException();
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            throw new NotImplementedException();
        }

        public Task<IFileInfo> SaveAsync(PersistFileInfo fileInfo, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public IChangeToken Watch(string filter)
        {
            throw new NotImplementedException();
        }
    }
}