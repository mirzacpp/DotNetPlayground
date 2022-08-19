using Studens.AspNetCore.Mvc.UI.Localization;
using Studens.Commons.Localization;

namespace Studens.AspNetCore.Mvc.UI.TagHelpers.Localization
{
	/// <summary>
	/// <see cref="ITagHelper"/> implementation targeting &lt;input&gt; elements with an <c>asp-for-localized</c> attribute.
	/// </summary>
	[HtmlTargetElement(HtmlTagNames.Input, Attributes = AspLocalizedAttributeName, TagStructure = TagStructure.NormalOrSelfClosing)]
	[HtmlTargetElement(HtmlTagNames.Textarea, Attributes = AspLocalizedAttributeName, TagStructure = TagStructure.NormalOrSelfClosing)]
	public class TranslationInputEditorTagHelper : TagHelper
	{
		private const string AspLocalizedAttributeName = "asp-for-localized";
		private readonly ILanguageProvider _languageProvider;
		private readonly IInputControlGenerator _inputControlGenerator;

		public TranslationInputEditorTagHelper(
		ILanguageProvider languageProvider,
		IInputControlGenerator inputControlGenerator)
		{
			_languageProvider = languageProvider;
			_inputControlGenerator = inputControlGenerator;
		}

		[HtmlAttributeName(AspLocalizedAttributeName)]
		public ModelExpression For { get; set; }

		[HtmlAttributeNotBound]
		[ViewContext]
		public ViewContext ViewContext { get; set; }

		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			// Throw if member is not of type TranslationModel
			if (For.Metadata.UnderlyingOrModelType != typeof(TranslationModel))
			{
				throw new InvalidOperationException($"Model is not of type {typeof(TranslationModel)}.");
			}

			//Preserve original html tag (input or textarea)
			var tagName = output.TagName;

			//Transform original input to wrapper
			output.TagName = HtmlTagNames.Div;
			output.TagMode = TagMode.StartTagAndEndTag;
			output.Attributes.Add(TagAttributeNames.Id, HtmlIdGenerator.GetRandomId("language-control"));

			// TODO: No languages check ?
			var languages = await _languageProvider.GetLanguagesAsync();
			// TODO: Implement get current language provider
			var currentLanguage = languages[0];

			var inputWrapper = _inputControlGenerator
			.Generate(new LocalizationControlContext(For, ViewContext, languages, currentLanguage, tagName));

			// Append all to parent
			output.Content.AppendHtml(inputWrapper);
			//TODO: We should move original element css to all input controls ...
			output.Attributes.RemoveAll("class");
		}
	}
}