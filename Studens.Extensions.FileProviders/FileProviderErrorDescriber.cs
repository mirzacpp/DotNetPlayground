namespace Studens.Extensions.FileProviders;

/// <summary>
/// Represents a service used to localize errors occured in file provider.
/// </summary>
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
}