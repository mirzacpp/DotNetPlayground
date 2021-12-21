using System;

namespace Cleaners.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="System.String"/> class
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Shorthand for value.ToString().ToLower()
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToLower(this object value) => value.ToString().ToLower();

        /// <summary>
        /// Shorthand for value.ToString().ToLowerInvariant()
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToLowerInvariant(this object value) => value.ToString().ToLowerInvariant();

        /// <summary>
        /// Shorthand for !string.IsNullOrEmpty(value)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNotNullOrEmpty(this string value) => !string.IsNullOrEmpty(value);

        /// <summary>
        /// Shorthand for string.IsNullOrEmpty(value)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);

        /// <summary>
        /// Shorthand for string.Format(value, args);
        /// </summary>
        /// <param name="value"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string Format(this string value, params object[] args) => string.Format(value, args);

        /// <summary>
        /// Converts specified string to enum
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <param name="value">Value to convert</param>
        /// <returns>Enum of T</returns>
        public static T ToEnum<T>(this string value) => ToEnum<T>(value, ignoreCase: false);

        /// <summary>
        /// Converts specified string to enum
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <param name="value">Value to convert</param>
        /// <param name="ignoreCase">Ignore case</param>
        /// <returns>Enum of T</returns>
        public static T ToEnum<T>(this string value, bool ignoreCase)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }

        /// <summary>
        /// Checks if specified string is valid enum
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <param name="value">Value to check</param>
        /// <returns>Check result</returns>
        public static bool IsValidEnum<T>(this string value)
            where T : struct => IsValidEnum<T>(value, ignoreCase: false);

        /// <summary>
        /// Checks if specified string is valid enum
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <param name="value">Value to check</param>
        /// <param name="ignoreCase">Ignore case</param>
        /// <returns>Check result</returns>
        public static bool IsValidEnum<T>(this string value, bool ignoreCase) where T : struct
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            return Enum.TryParse(value, ignoreCase, out T _);
        }        
    }
}