using Ardalis.GuardClauses;
using System.Collections.Generic;

namespace Cleaners.Extensions
{
    public static class ListExtensions
    {
        /// <summary>
        /// Enables collection intialization which can be useful with read-only properties
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="collection"></param>
        public static void Add<T>(this List<T> list, IEnumerable<T> collection)
        {
            Guard.Against.Null(collection, nameof(collection));

            list.AddRange(collection);
        }
    }
}