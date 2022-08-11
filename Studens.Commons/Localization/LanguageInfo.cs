using Studens.Commons.Extensions;
using System.Globalization;

namespace Studens.Commons.Localization
{
	public class LanguageInfo : ILanguageInfo
	{
		/// <summary>
		/// Predefined English language(en)
		/// </summary>
		public static readonly LanguageInfo EnglishCulture = new("en", "en", "English", "en");

		public string CultureName { get; protected set; }
		public string UiCultureName { get; protected set; }
		public string DisplayName { get; protected set; }
		public bool IsRtl { get; protected set; }
		public string? FlagIcon { get; protected set; }

		public LanguageInfo(string cultureName,
		string? uiCultureName = null,
		string? displayName = null,
		string? flagIcon = null)
		{
			CultureName = cultureName;
			UiCultureName = uiCultureName.IsNotNullOrEmpty() ? uiCultureName : cultureName;
			DisplayName = displayName.IsNotNullOrEmpty() ? displayName : cultureName;
			FlagIcon = flagIcon;
			IsRtl = new CultureInfo(cultureName).TextInfo.IsRightToLeft;
		}
	}
}