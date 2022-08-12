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

		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			// Throw if property is not of type TranslationModel
			if (For.Metadata.UnderlyingOrModelType != typeof(TranslationModel))
			{
				throw new InvalidOperationException($"Member is not of type {typeof(TranslationModel)}.");
			}

			//Preserve original html tag (input or textarea)
			var tagName = output.TagName;

			//Transform original input to wrapper
			output.TagName = HtmlTagNames.Div;
			output.TagMode = TagMode.StartTagAndEndTag;
			//TODO: Use guid if UniqueId is too long or use some random 5/6 digit number?
			output.Attributes.Add(TagAttributeNames.Id, "language-control-" + Guid.NewGuid().ToString("N"));

			// TODO: No languages check ?
			var languages = await _languageProvider.GetLanguagesAsync();
			var currentLanguage = languages[0];

			var inputWrapper = _inputControlGenerator
			.Generate(new LocalizationControlContext(For, languages, currentLanguage, tagName));

			// Append all to parent
			output.Content.AppendHtml(inputWrapper);
			//TODO: We should move original element css to all input controls ...
			output.Attributes.RemoveAll("class");
		}
	}
}