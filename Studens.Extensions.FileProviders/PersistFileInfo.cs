using Microsoft.Extensions.FileProviders;

namespace Studens.Extensions.FileProviders
{
    /// <summary>
    /// Represents file info used for persistance
    /// </summary>
    public class PersistFileInfo : IFileInfo
    {
        #region Ctor

        public PersistFileInfo(byte[] content, string name)
        {
            _content = content;
            Name = name;
            LastModified = DateTimeOffset.Now;
        }

        public PersistFileInfo(byte[] content, string name, string path)
            : this(content, name)
        {
            Path = path;
        }

        public PersistFileInfo(byte[] content, string name, string path, bool overwriteExisting)
            : this(content, name, path)
        {
            OverwriteExisting = overwriteExisting;
        }

        #endregion Ctor

        /// <summary>
        /// Stores content for current file.
        /// </summary>
        private readonly byte[] _content;

        /// <summary>
        /// Represents file path
        /// </summary>
        public string Path { get; } = string.Empty;

        public bool OverwriteExisting { get; }

        public bool Exists => true;

        public long Length => _content.Length;

        public string? PhysicalPath => null!;

        public string Name { get; }

        public DateTimeOffset LastModified { get; }

        public bool IsDirectory => false;

        public Stream CreateReadStream() =>
            new MemoryStream(_content, writable: false);
    }
}