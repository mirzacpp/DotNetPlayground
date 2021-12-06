using System;
using System.Collections.Generic;
using System.Linq;

namespace Cleaners.Utils
{
    /// <summary>
    /// Additional methods for <see cref="System.Enum"/> class
    /// </summary>
    public static class EnumUtils
    {
        [Obsolete("Switch to internal Enum method.")]
        /// <summary>
        /// Shorther way to retrieve collection of Enum names
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <returns>Enum items</returns>
        public static IEnumerable<string> GetEnumNames<T>() where T : Enum
            => Enum.GetNames(typeof(T));

        [Obsolete("Switch to internal Enum method.")]
        /// <summary>
        /// Shorther way to retrieve collection of Enum values
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <returns>Enum items</returns>
        public static IEnumerable<T> GetEnumValues<T>() where T : Enum
            => Enum.GetValues(typeof(T)).Cast<T>();
    }
}