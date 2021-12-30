using Microsoft.Extensions.FileProviders;

namespace Studens.Extensions.FileProviders
{
    /// <summary>
    /// Represents file info used for persistance
    /// </summary>
    public class PersistFileInfo : IFileInfo
    {
        /// <summary>
        /// Stores content for current file.
        /// </summary>
        private readonly byte[] _content;

        public string Subpath { get; }

        public bool OverwriteExisting { get; }

        public bool Exists => true;

        public long Length => _content.Length;

        public string? PhysicalPath => null!;

        public string Name { get; }

        public DateTimeOffset LastModified { get; }

        public bool IsDirectory => false;

        public PersistFileInfo(string subpath, byte[] content, string name, bool overwriteExisting = false)
        {
            _content = content;
            Subpath = subpath;
            Name = name;
            OverwriteExisting = overwriteExisting;
            LastModified = DateTimeOffset.Now;
        }

        public Stream CreateReadStream() =>
            new MemoryStream(_content, writable: false);
    }
}