using System.Text;

namespace Studens.Commons.Extensions;

/// <summary>
/// Extension methods for <see cref="string"/> class
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Shorthand for value.ToString().ToLower()
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToLower(this string value) => value.ToString().ToLower();

    /// <summary>
    /// Shorthand for value.ToString().ToLowerInvariant()
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToLowerInvariant(this string value) => value.ToString().ToLowerInvariant();

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
    public static T ToEnum<T>(this string value) => value.ToEnum<T>(ignoreCase: false);

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
        where T : struct => value.IsValidEnum<T>(ignoreCase: false);

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

    /// <summary>
    /// Encodes a givne string to a base64 value
    /// </summary>
    /// <param name="value">Value to encode</param>
    /// <returns>Encoded base64</returns>
    public static string EncodeBase64(this string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// Decodes a givne base64 to a string value
    /// </summary>
    /// <param name="value">Value to decode</param>
    /// <returns>Decoded text</returns>
    public static string DecodeBase64(this string value)
    {
        var bytes = Convert.FromBase64String(value);
        return Encoding.UTF8.GetString(bytes);
    }
}