using System.Globalization;

namespace Studens.Commons.Localization
{
	/// <summary>
	/// Defines localization options.
	/// </summary>
	public class LocalizationOptions
	{
		/// <summary>
		/// Gets or sets the <see cref="CultureInfo"/> used for formatting.
		/// Defaults to <see cref="CultureInfo.CurrentCulture"/>.
		/// </summary>
		public CultureInfo DefaultCulture { get; set; } = CultureInfo.CurrentCulture;

		/// <summary>
		/// Gets or sets the <see cref="CultureInfo"/> used for text, i.e. language;
		/// Defaults to <see cref="CultureInfo.CurrentUICulture"/>.
		/// </summary>
		public CultureInfo DefaultUICulture { get; set; } = CultureInfo.CurrentUICulture;

		/// <summary>
		/// Gets or sets depth of parent culture fallback.
		/// Defaults to <c>5</c>
		/// </summary>
		public int MaxCultureFallbackDepth { get; set; } = 5;

		/// <summary>
		/// Determines whether should culture in case it is not supported, fall back to its parent culture.
		/// Ie. en-GB will fall back to en etc.
		/// </summary>
		public bool FallBackToParentCultures { get; set; } = true;

		/// <summary>
		/// The cultures supported by the application.
		/// Defaults to <see cref="CultureInfo.CurrentCulture"/>.
		/// </summary>
		public IList<CultureInfo>? SupportedCultures { get; set; } = new List<CultureInfo> { CultureInfo.CurrentCulture };

		/// <summary>
		/// The UI cultures supported by the application.
		/// Defaults to <see cref="CultureInfo.CurrentUICulture"/>.
		/// </summary>
		public IList<CultureInfo>? SupportedUICultures { get; set; } = new List<CultureInfo> { CultureInfo.CurrentUICulture };
	}
}