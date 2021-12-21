namespace Studens.Commons.Extensions;

/// <summary>
/// Extension methods for <see cref="List{T}"/>
/// </summary>
public static class ListExtensions
{
    /// <summary>
    /// Enables collection intialization which can be useful with read-only properties
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list">List to extend</param>
    /// <param name="collection">Collection to append</param>
    public static void Add<T>(this List<T> list, IEnumerable<T> collection)
    {
        Guard.Against.Null(collection, nameof(collection));

        list.AddRange(collection);
    }
}
