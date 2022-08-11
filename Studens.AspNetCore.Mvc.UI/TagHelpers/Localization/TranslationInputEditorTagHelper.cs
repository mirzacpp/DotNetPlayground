using Studens.AspNetCore.Mvc.UI.Localization;
using Studens.Commons.Localization;

namespace Studens.AspNetCore.Mvc.UI.TagHelpers.Localization
{
	[HtmlTargetElement(HtmlTagNames.Input, Attributes = AspLocalizedAttributeName, TagStructure = TagStructure.NormalOrSelfClosing)]
	[HtmlTargetElement(HtmlTagNames.Textarea, Attributes = AspLocalizedAttributeName, TagStructure = TagStructure.NormalOrSelfClosing)]
	public class TranslationInputEditorTagHelper : TagHelper
	{
		private const string AspLocalizedAttributeName = "asp-for-localized";
		private readonly ILanguageProvider _languageProvider;
		private readonly IInputControlGenerator _inputControlGenerator;

		public TranslationInputEditorTagHelper(
		IHtmlGenerator generator,
		ILanguageProvider languageProvider,
		IInputControlGenerator inputControlGenerator)
		{
			Generator = generator;
			_languageProvider = languageProvider;
			_inputControlGenerator = inputControlGenerator;
		}

		[HtmlAttributeName(AspLocalizedAttributeName)]
		public ModelExpression For { get; set; }

		/// <summary>
		/// Gets the <see cref="IHtmlGenerator"/> used to generate the <see cref="InputTagHelper"/>'s output.
		/// </summary>
		protected IHtmlGenerator Generator { get; }

		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			// Throw if property is not of type TranslationModel
			if (For.Metadata.UnderlyingOrModelType != typeof(TranslationModel))
			{
				throw new InvalidOperationException($"Member is not of type {typeof(TranslationModel)}");
			}

			//TODO: Add check if there are no available languages. Should we inherit InputTagHelper and just invoke it ?
			// One language should always be present if localization is configured,
			// but instead of dropdown we can just generate basic input group?

			//Preserve original html tag (input or textarea)
			var tagName = output.TagName;

			//Transform original input to wrapper
			output.TagName = HtmlTagNames.Div;
			output.TagMode = TagMode.StartTagAndEndTag;
			//TODO: Use guid if UniqueId is too long or use some random 5/6 digit number?
			output.Attributes.Add(TagAttributeNames.Id, "input-group-localized-" + Guid.NewGuid().ToString("N"));

			var languages = await _languageProvider.GetLanguagesAsync();
			var currentLanguage = languages.First();

			var inputWrapper = _inputControlGenerator
			.Generate(new LocalizationControlContext(For, languages, currentLanguage, tagName));

			// Append all to parent
			output.Content.AppendHtml(inputWrapper);
			//TODO: We should move original element css to all input controls ...
			output.Attributes.RemoveAll("class");
		}
	}
}