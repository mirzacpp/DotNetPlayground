namespace Studens.Extensions.FileProviders
{
    /// <summary>
    /// Base implementation for <see cref="IPersistFileInfo"/>
    /// </summary>
    public abstract class PersistFileInfoBase : IPersistFileInfo
    {
        public PersistFileInfoBase(byte[] content, string name, string subpath, bool overwriteExisting)
        {
            _content = content;
            Name = name;
            Subpath = subpath;
            OverwriteExisting = overwriteExisting;
        }

        public PersistFileInfoBase(byte[] content, string name, string subpath)
            : this(content, name, subpath, true)
        {
        }

        private readonly byte[] _content;

        public string Name { get; }

        public string Subpath { get; }

        public bool OverwriteExisting { get; }

        public virtual Stream CreateReadStream() => new MemoryStream(_content, writable: false);
    }
}