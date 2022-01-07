using Ardalis.GuardClauses;

namespace Studens.Extensions.FileProviders.Amazon;

/// <summary>
/// Path utilities
/// </summary>
internal static class PathUtils
{
    internal static readonly char _pathSeparator = '/';

    internal static string EnsureTrailingSlash(string path)
    {
        if (!string.IsNullOrEmpty(path) && path[^1] != _pathSeparator)
        {
            return path + _pathSeparator;
        }

        return path;
    }

    internal static string Combine(string path1, string path2)
    {
        Guard.Against.Null(path1, nameof(path1));
        Guard.Against.Null(path2, nameof(path2));

        if (string.IsNullOrEmpty(path2))
        {
            return path1;
        }

        if (string.IsNullOrEmpty(path1))
        {
            return path2;
        }

        return EnsureTrailingSlash(path1) + path2;
    }
}