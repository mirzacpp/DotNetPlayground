﻿using System.Globalization;

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
		public LanguageInfo DefaultLanguage { get; set; } = LanguageInfo.EnglishCulture;		

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
		/// The languages supported by the application.
		/// Defaults to <see cref="CultureInfo.CurrentCulture"/>.
		/// </summary>
		public IList<LanguageInfo> Languages { get; set; } = new List<LanguageInfo> { LanguageInfo.EnglishCulture };		
	}
}