namespace System;

public static class BooleanExtensions
{
    /// <summary>
    /// Returns bool as lowercased string.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToStringLowercase(this bool value)
        => value ? "true" : "false";
}