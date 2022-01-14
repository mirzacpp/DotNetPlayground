namespace Studens.Extensions.FileProviders;

/// <summary>
/// Represents a service used to localize errors occured in file provider.
/// </summary>
/// <remarks>
/// TODO: Add extenion method to replace error describer with custom type
/// </remarks>
public class FileProviderErrorDescriber
{
    public virtual FileProviderError DefaultError() =>
        new(nameof(DefaultError), "Unexpected error occured.");

    public virtual FileProviderError InvalidPath() =>
        new(nameof(InvalidPath), "Path is not valid.");

    public virtual FileProviderError PathTooLong() =>
        new(nameof(PathTooLong), "Path exceeded maximum allowed length.");

    public virtual FileProviderError FileInUse() =>
        new(nameof(FileInUse), "File in use.");

    public virtual FileProviderError DirectoryNotFound() =>
        new(nameof(DirectoryNotFound), "Directory not found.");

    public virtual FileProviderError DriveNotFound() =>
        new(nameof(DriveNotFound), "The drive specified in 'path' is invalid.");

    public virtual FileProviderError FileNotFound(string? fileName) =>
        new(nameof(DirectoryNotFound), $"File with name '{fileName}' not found.");

    public virtual FileProviderError UnauthorizedAccess() =>
        new(nameof(UnauthorizedAccess), "Unauthorized access.");

    public virtual FileProviderError FileExists() =>
        new(nameof(FileExists), "File already exists.");

    public virtual FileProviderError SharingViolation() =>
        new(nameof(SharingViolation), "The file name is missing, or the file or directory is in use.");
}