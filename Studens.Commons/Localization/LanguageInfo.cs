using Studens.Commons.Extensions;

namespace Studens.Commons.Localization
{
	public class LanguageInfo : ILanguageInfo
	{
		public string CultureName { get; protected set; }
		public string UiCultureName { get; protected set; }
		public string DisplayName { get; protected set; }
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
		}
	}
}