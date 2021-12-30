using Microsoft.Extensions.Primitives;

namespace Studens.Extensions.FileProviders.FileSystem;

/// <summary>
/// Code from <see cref="https://github.com/dotnet/runtime/blob/main/src/libraries/Microsoft.Extensions.FileProviders.Physical/src/Internal/PathUtils.cs"/>
/// </summary>
internal static class PathUtils
{
    private static readonly char[] _invalidFileNameChars = Path.GetInvalidFileNameChars()
        .Where(c => c != Path.DirectorySeparatorChar && c != Path.AltDirectorySeparatorChar).ToArray();

    private static readonly char[] _invalidFilterChars = _invalidFileNameChars
        .Where(c => c != '*' && c != '|' && c != '?').ToArray();

    private static readonly char[] _pathSeparators = new[]
        {Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar};

    internal static bool HasInvalidPathChars(string path)
    {
        return path.IndexOfAny(_invalidFileNameChars) != -1;
    }

    internal static bool HasInvalidFilterChars(string path)
    {
        return path.IndexOfAny(_invalidFilterChars) != -1;
    }

    internal static string EnsureTrailingSlash(string path)
    {
        if (!string.IsNullOrEmpty(path) &&
            path[path.Length - 1] != Path.DirectorySeparatorChar)
        {
            return path + Path.DirectorySeparatorChar;
        }

        return path;
    }

    internal static bool PathNavigatesAboveRoot(string path)
    {
        var tokenizer = new StringTokenizer(path, _pathSeparators);
        int depth = 0;

        foreach (StringSegment segment in tokenizer)
        {
            if (segment.Equals(".") || segment.Equals(""))
            {
                continue;
            }
            else if (segment.Equals(".."))
            {
                depth--;

                if (depth == -1)
                {
                    return true;
                }
            }
            else
            {
                depth++;
            }
        }

        return false;
    }
}