using Studens.Commons.Localization;

namespace Studens.AspNetCore.Mvc.UI.TagHelpers.Localization
{
	public class LocalizationControlContext
	{
		public ModelExpression For { get; }
		public IReadOnlyList<LanguageInfo> Languages { get; }
		public LanguageInfo CurrentLanguage { get; }
		public string TagName { get; set; }

		public LocalizationControlContext(
		ModelExpression @for,
		IReadOnlyList<LanguageInfo> languages,
		LanguageInfo currentLanguage,
		string tagName)
		{
			For = @for;
			Languages = languages;
			CurrentLanguage = currentLanguage;
			TagName = tagName;
		}
	}
}