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
        /// <summary>
        /// Shorther way to retrieve all enum names
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <returns>Enum items</returns>
        public static IEnumerable<T> GetEnumNames<T>() where T : Enum
            => Enum.GetNames(typeof(T)).Cast<T>();
    }
}