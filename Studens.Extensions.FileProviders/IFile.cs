namespace Studens.Extensions.FileProviders
{
    /// <summary>
    /// Represents a file
    /// </summary>
    public interface IFile
    {
        /// <summary>
        /// Represents a file name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Represents a file path
        /// </summary>
        string? Path { get; }

        /// <summary>
        /// Last date of file modification
        /// </summary>
        DateTimeOffset LastModified { get; }

        /// <summary>
        /// The length of the file in bytes, or -1 for a directory or non-existing files.
        /// </summary>
        public long Length { get; }
    }
}