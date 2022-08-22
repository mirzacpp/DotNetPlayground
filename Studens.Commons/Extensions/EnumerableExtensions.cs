namespace Studens.Commons.Extensions;

/// <summary>
/// Extension methods for <see cref="IEnumerable{T}"/>
/// </summary>
public static class EnumerableExtensions
{
    // Simply returns empty collection if source is currently null
    // This way we avoid constantly checking if source != null && source.Count > 0 etc.
    public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source)
        => source ?? Enumerable.Empty<T>();
}
