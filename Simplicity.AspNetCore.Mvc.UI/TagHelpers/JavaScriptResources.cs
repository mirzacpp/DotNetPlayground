using System.Collections.Concurrent;
using System.Reflection;

namespace Simplicity.AspNetCore.Mvc.UI.TagHelpers
{
	/// <summary>
	/// Methods for loading JavaScript from assembly embedded resources.
	/// Source: https://github.com/dotnet/aspnetcore/blob/c85baf8db0c72ae8e68643029d514b2e737c9fae/src/Mvc/Mvc.TagHelpers/src/JavaScriptResources.cs
	/// </summary>
	internal static class JavaScriptResources
	{
		private static readonly Assembly ResourcesAssembly = typeof(JavaScriptResources).Assembly;

		private static readonly ConcurrentDictionary<string, string> Cache = new(StringComparer.Ordinal);

		/// <summary>
		/// Gets an embedded JavaScript file resource and decodes it for use as a .NET format string.
		/// </summary>
		public static string GetEmbeddedJavaScript(string resourceName)
		{
			return GetEmbeddedJavaScript(resourceName, ResourcesAssembly.GetManifestResourceStream, Cache);
		}

		// Internal for testing
		internal static string GetEmbeddedJavaScript(
			string resourceName,
			Func<string, Stream> getManifestResourceStream,
			ConcurrentDictionary<string, string> cache)
		{
			return cache.GetOrAdd(resourceName, key =>
			{
				// Load the JavaScript from embedded resource
				using var resourceStream = getManifestResourceStream(key);
				using var streamReader = new StreamReader(resourceStream);
				var script = streamReader.ReadToEnd();

				return script;
			});
		}

		private static string PrepareFormatString(string input) =>
			// Remove final ");". Those characters are in the file only to allow minification.
			input[..^2];
	}
}