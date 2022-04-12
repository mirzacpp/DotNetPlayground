namespace Studens.Extensions.FileProviders
{
    /// <summary>
    /// Represents a file that could not be found.
    /// </summary>
    public class NotFoundFile : IFile
    {
        public NotFoundFile(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public string? Path => null;
        public DateTimeOffset LastModified => DateTimeOffset.MinValue;
        public long Length => -1;
    }
}