using System.Collections.Generic;
using System.Linq;

namespace Cleaners.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="System.Collections.Generic.IEnumerable{T}"/>
    /// </summary>
    public static class EnumerableExtensions
    {
        // Simply returns empty collection if source is currently null
        // This way we avoid constantly checking if source != null && source.Count > 0 etc.
        public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T> source)
            => source ?? Enumerable.Empty<T>();
    }
}