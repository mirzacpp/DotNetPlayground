using Simplicity.Commons.Localization;

namespace Simplicity.AspNetCore.Mvc.UI.TagHelpers.Localization
{
	public class LocalizationControlContext
	{
		public ModelExpression For { get; }
		public ViewContext ViewContext { get; }
		public IReadOnlyList<LanguageInfo> Languages { get; }
		public LanguageInfo CurrentLanguage { get; }
		public string TagName { get; }

		public LocalizationControlContext(
		ModelExpression @for,
		ViewContext viewContext,
		IReadOnlyList<LanguageInfo> languages,
		LanguageInfo currentLanguage,
		string tagName)
		{
			For = @for;
			ViewContext = viewContext;
			Languages = languages;
			CurrentLanguage = currentLanguage;
			TagName = tagName;
		}
	}
}