namespace Studens.Commons;

/// <summary>
/// Implements GUID generation using built-in methods
/// </summary>
public class DefaultGuidGenerator : IGuidGenerator
{
    /// <summary>
    /// Static instance when DI is not possible
    /// </summary>
    public static DefaultGuidGenerator Instance => new();

    public Guid Generate() => Guid.NewGuid();
}
