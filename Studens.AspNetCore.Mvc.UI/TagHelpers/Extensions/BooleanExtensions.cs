namespace System;

internal static class BooleanExtensions
{
    /// <summary>
    /// Returns bool as lowercased string.
    /// </summary>
    /// <param name="value">Booelan value</param>
    /// <returns>Boolean as string</returns>
    public static string ToStringLowercase(this bool value)
        => value ? "true" : "false";
}