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
        string Path { get; }

        /// <summary>
        /// Last date of file modification
        /// </summary>
        DateTimeOffset LastModified { get; }
    }
}