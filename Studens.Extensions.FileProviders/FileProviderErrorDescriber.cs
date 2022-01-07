namespace Studens.Extensions.FileProviders;

/// <summary>
/// Represents a service used to localize errors occured in file provider.
/// </summary>
/// <remarks>
/// TODO: Add extenion method to replace error describer with custom type
/// </remarks>
public class FileProviderErrorDescriber
{
    public virtual FileProviderError DefaultError() => new()
    {
        Code = nameof(DefaultError),
        Description = "Unexpected error occured."
    };

    public virtual FileProviderError InvalidPath() => new()
    {
        Code = nameof(InvalidPath),
        Description = "Path is not valid."
    };

    public virtual FileProviderError PathTooLong() => new()
    {
        Code = nameof(PathTooLong),
        Description = "Path exceeded maximum allowed length."
    };

    public virtual FileProviderError FileInUse() => new()
    {
        Code = nameof(FileInUse),
        Description = "File in use."
    };

    public virtual FileProviderError DirectoryNotFound() => new()
    {
        Code = nameof(DirectoryNotFound),
        Description = "Directory not found."
    };

    public virtual FileProviderError DriveNotFound() => new()
    {
        Code = nameof(DriveNotFound),
        Description = "The drive specified in 'path' is invalid."
    };

    public virtual FileProviderError FileNotFound(string? fileName) => new()
    {
        Code = nameof(DirectoryNotFound),
        Description = $"File with name '{fileName}' not found."
    };

    public virtual FileProviderError UnauthorizedAccess() => new()
    {
        Code = nameof(UnauthorizedAccess),
        Description = "Unauthorized access."
    };

    public virtual FileProviderError FileExists() => new()
    {
        Code = nameof(FileExists),
        Description = "File already exists."
    };

    public virtual FileProviderError SharingViolation() => new()
    {
        Code = nameof(SharingViolation),
        Description = "The file name is missing, or the file or directory is in use."
    };
}