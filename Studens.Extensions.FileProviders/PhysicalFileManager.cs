using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Studens.Commons.Extensions;

namespace Studens.Extensions.FileProviders
{
    public class PhysicalFileManager : IFileManager
    {
        /// <summary>
        /// Instance of built-in <see cref="PhysicalFileProvider"/>
        /// </summary>
        private readonly IFileProvider _fileProvider;

        //private readonly ILogger<PhysicalFileManager> _logger;

        private string Root { get; init; }

        public PhysicalFileManager(IOptions<PhysicalFileManagerOptions> optionsAccessor)
        {
            _fileProvider = new PhysicalFileProvider(optionsAccessor.Value.Path);
            Root = optionsAccessor.Value.Path;
        }

        public async Task<string> SaveAsync(PersistFileInfo fileInfo, CancellationToken cancellationToken = default)
        {
            if (fileInfo is null)
            {
                throw new ArgumentNullException(nameof(fileInfo));
            }

            var path = Path.Combine(Root, fileInfo.Subpath);

            EnsureDirectoryExists(path);

            using var stream = fileInfo.CreateReadStream();
            var bytes = await stream.GetAllBytesAsync(cancellationToken);
            await File.WriteAllBytesAsync(path, bytes, cancellationToken);

            return path;
        }

        #region Read

        /// <inheritdoc/>
        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            return _fileProvider.GetDirectoryContents(subpath);
        }

        /// <inheritdoc/>
        public IFileInfo GetFileInfo(string subpath)
        {
            return _fileProvider.GetFileInfo(subpath);
        }

        /// <inheritdoc/>
        public IChangeToken Watch(string filter)
        {
            return _fileProvider.Watch(filter);
        }

        #endregion Read

        private void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}