namespace Studens.Extensions.FileProviders
{
    /// <summary>
    /// Represents an file to persist.
    /// </summary>
    public interface IPersistFileInfo
    {
        /// <summary>
        /// Name of the file
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Path that file should be stored to.
        /// </summary>
        /// <remarks>
        /// Should be relative path
        /// </remarks>
        string Subpath { get; }

        /// <summary>
        /// Indicates if existing file should be overwriten.
        /// </summary>
        bool OverwriteExisting { get; }

        /// <summary>
        /// Return file contents as readonly stream. Caller should dispose stream when complete.
        /// </summary>
        /// <returns>The file stream</returns>
        Stream CreateReadStream();
    }
}