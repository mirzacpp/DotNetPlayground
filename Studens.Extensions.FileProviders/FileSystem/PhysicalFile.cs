using Microsoft.Extensions.FileProviders;

namespace Studens.Extensions.FileProviders.FileSystem
{
    /// <summary>
    /// Represents an Physchical file implementation
    /// </summary>
    public class PhysicalFile : IFile
    {
        public PhysicalFile(IFileInfo fileInfo)
        {
            Name = fileInfo.Name;
            Path = fileInfo.PhysicalPath;
            LastModified = fileInfo.LastModified;
            Length = fileInfo.Length;
        }

        public string Name { get; }
        public string? Path { get; }
        public DateTimeOffset LastModified { get; }
        public long Length { get; }
    }
}