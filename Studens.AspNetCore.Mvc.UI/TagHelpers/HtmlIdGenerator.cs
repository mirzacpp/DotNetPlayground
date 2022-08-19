namespace Studens.AspNetCore.Mvc.UI.TagHelpers
{
	/// <summary>
	/// Helper class for generating random id attribute values.
	/// </summary>
	public static class HtmlIdGenerator
	{
		/// <summary>
		/// Generates random id value based on provided prefix value(Could be an html tag name, value prefix etc.) and random 5 digit number.
		/// <param name="prefix">Tag name</param>
		/// </summary>
		/// <returns>Random id value</returns>
		/// <remarks>
		/// Note that we use shared instance of Random introduced in .NET6 which is thread safe,
		/// even though this is not critical since we only use it generate random html ids that can be same for different users.
		/// For < .NET6 versions, <see cref="Commons.Random.CryptoRandom"/>.
		/// </remarks>
		public static string GetRandomId(string prefix) =>
		$"{prefix ?? "element"}-{Random.Shared.NextInt64(10000, 99999)}";
	}
}