namespace Studens.Extensions.FileProviders
{
    /// <summary>
    /// Represents an file provider error.
    /// </summary>
    public class FileProviderError
    {
        public FileProviderError(string code, string description)
        {
            Code = code;
            Description = description;
        }

        /// <summary>
        /// Gets or sets the code for this error.
        /// </summary>
        /// <value>
        /// The code for this error.
        /// </value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the description for this error.
        /// </summary>
        /// <value>
        /// The description for this error.
        /// </value>
        public string Description { get; set; }
    }
}