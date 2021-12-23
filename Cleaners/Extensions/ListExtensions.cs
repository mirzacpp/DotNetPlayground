using Ardalis.GuardClauses;
using System.Collections.Generic;

namespace Cleaners.Extensions
{
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

        /// <summary>
        /// Inserts element on the beginning of the list
        /// </summary>
        public static void AddFirst<T>(this IList<T> source, T item)
        {
            source.Insert(0, item);
        }

        /// <summary>
        /// Appends items to an existing collection if condition is satisfied
        /// </summary>
        public static void AddRangeIf<T>(this List<T> source, bool condition, List<T> items)
        {
            if (condition)
            {
                source.AddRange(items);
            }
        }

        /// <summary>
        /// Appends item to an existing collection if condition is satisfied
        /// </summary>
        public static void AddIf<T>(this List<T> source, bool condition, T item)
        {
            if (condition)
            {
                source.Add(item);
            }
        }        
    }
}