using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Studens.AspNetCore.Mvc.UI.Localization;
using System.Globalization;

namespace Studens.AspNetCore.Mvc.UI.TagHelpers.Localization
{
	[HtmlTargetElement("input", Attributes = "asp-for-localized", TagStructure = TagStructure.NormalOrSelfClosing)]
	[HtmlTargetElement("textarea", Attributes = "asp-for-localized", TagStructure = TagStructure.NormalOrSelfClosing)]
	public class TranslationInputEditorTagHelper : TagHelper
	{
		public TranslationInputEditorTagHelper(IHtmlGenerator generator)
		{
			Generator = generator;
		}

		[HtmlAttributeName("asp-for-localized")]
		public ModelExpression For { get; set; }

		/// <summary>
		/// Gets the <see cref="IHtmlGenerator"/> used to generate the <see cref="InputTagHelper"/>'s output.
		/// </summary>
		protected IHtmlGenerator Generator { get; }

		public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			// Throw if property is not of type TranslationModel
			if (For.Metadata.UnderlyingOrModelType != typeof(TranslationModel))
			{
				throw new InvalidOperationException($"Member is not of type {typeof(TranslationModel)}");
			}

			//TODO: Add check if there are no available languages. Should we inherit InputTagHelper and just invoke it ?

			//Preserve original html tag (input or textarea)
			var tagName = output.TagName;

			//Transform original input to wrapper
			output.TagName = "div";
			output.TagMode = TagMode.StartTagAndEndTag;
			//TODO: Use guid if UniqueId is too long or use some random 5/6 digit number?
			output.Attributes.Add("id", "input-group-localized-" + Guid.NewGuid().ToString("N"));

			// Create parent input group container
			var inputGroupTag = new TagBuilder("div");
			inputGroupTag.AddCssClass("input-group mb-3");

			var langs = new List<string> { "en", "bs", "de", "it" };
			var currentLang = "bs";

			// TODO: Need language service to get all available languages and to get current language
			inputGroupTag.InnerHtml
			.AppendHtml(@$"<button class=""btn btn-outline-secondary dropdown-toggle"" type=""button"" data-bs-toggle=""dropdown"" aria-expanded=""false"">
				<span class=""fi fi-{currentLang} fis""></span>
			</button>")
			.AppendHtml(GenerateLanguageDropDown(langs));

			for (int i = 0; i < langs.Count; i++)
			{
				var lang = langs[i];
				var cultureInfo = new CultureInfo(lang);

				var inputValueId = $"{For.Name}_{nameof(TranslationModel.Translations)}_{i}_{nameof(TranslationEntryModel.Value)}";
				var inputValueName = $"{For.Name}.{nameof(TranslationModel.Translations)}[{i}].{nameof(TranslationEntryModel.Value)}";

				// Create input for lang code
				var inputLangCodeId = $"{For.Name}_{nameof(TranslationModel.Translations)}_{i}_{nameof(TranslationEntryModel.LangCode)}";
				var inputLangCodeName = $"{For.Name}.{nameof(TranslationModel.Translations)}[{i}].{nameof(TranslationEntryModel.LangCode)}";

				//var test = Generator.GenerateTextBox()

				var input = new TagBuilder(tagName);
				input.AddCssClass("form-control localized");
				//input.Attributes.Add("type", "text");
				input.Attributes.Add(key: "id", inputValueId);
				input.Attributes.Add("name", inputValueName);
				input.Attributes.Add("lang", lang);

				if (tagName == "input")
				{
					input.Attributes.Add("value", cultureInfo.NativeName);
					input.Attributes.Add("type", lang == currentLang ? "text" : "hidden");
				}
				else
				{
					input.InnerHtml.AppendHtml(cultureInfo.NativeName);
					if (lang != currentLang)
					{
						input.Attributes.Add("hidden", "hidden");
					}
				}

				inputGroupTag.InnerHtml.AppendHtml(input);

				//Generate hidden input for language
				var langInput = new TagBuilder("input");
				langInput.Attributes.Add(key: "id", inputLangCodeId);
				langInput.Attributes.Add("name", inputLangCodeName);
				langInput.Attributes.Add("value", lang);
				langInput.Attributes.Add("type", "hidden");

				inputGroupTag.InnerHtml.AppendHtml(langInput);
			}

			// Append all to parent
			output.Content.AppendHtml(inputGroupTag);
			//TODO: We should move original element css to all input controls ...
			output.Attributes.RemoveAll("class");

			return base.ProcessAsync(context, output);
		}

		private static TagBuilder GenerateLanguageDropDown(IEnumerable<string> langs)
		{
			var ulTag = new TagBuilder("ul");
			ulTag.AddCssClass("dropdown-menu");
			ulTag.Attributes.Add("id", "dropdown-lang"); // Combine with parent guid ?

			// Append languages. No need for TagBuilder here ...
			foreach (var lang in langs)
			{
				ulTag.InnerHtml.AppendHtml(@$"<li lang=""{lang}""><button type=""button"" class=""dropdown-item""><span class=""fi fi-{lang} fis""></span> {lang}</button></li>");
			}

			return ulTag;
		}
	}
}