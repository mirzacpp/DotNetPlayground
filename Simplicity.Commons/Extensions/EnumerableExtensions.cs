namespace Simplicity.Commons.Extensions;

/// <summary>
/// Extension methods for <see cref="IEnumerable{T}"/>
/// </summary>
public static class EnumerableExtensions
{
	// Simply returns empty collection if source is currently null
	// This way we avoid constantly checking if source != null && source.Count > 0 etc.
	public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source)
		=> source ?? Enumerable.Empty<T>();

	/// <summary>
	/// Checks whether <paramref name="source"/> contains duplicate elemenents.
	/// </summary>
	/// <typeparam name="T">Type of the collection</typeparam>
	/// <param name="source">Collection to check</param>
	public static bool ContainsDuplicates<T>(this IEnumerable<T> source)
	{
		var hashSet = new HashSet<T>();

		foreach (var item in source)
		{
			if (!hashSet.Add(item))
			{
				return true;
			}
		}

		return false;
	}

	/// <summary>
	/// Shorthand method for <see cref="string.Join{T}(string?, IEnumerable{T})"/>.
	/// </summary>
	public static string JoinToString<T>(this IEnumerable<T> source, string separator = ", ")
	=> string.Join(separator, source);
}