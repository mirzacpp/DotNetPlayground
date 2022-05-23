namespace System;

/// <summary>
/// MVC based extension methods for <see cref="string"/>
/// </summary>
public static partial class StringExtensions
{
    /// <summary>
    /// Returns controller name without "Controller" suffix.
    /// Used mostly in combination with nameof
    /// </summary>
    /// <param name="value">Controller name</param>
    /// <returns>Controller name without suffix</returns>
    public static string ToControllerName(this string value) =>
        value.Remove(value.LastIndexOf("Controller", StringComparison.Ordinal));
}