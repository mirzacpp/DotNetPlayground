using Studens.AspNetCore.Mvc.UI.Localization;
using Studens.Commons.Localization;

namespace Studens.AspNetCore.Mvc.UI.TagHelpers.Localization
{
	/// <summary>
	/// Generates translatable input based on bootstrap 5 input group components.
	/// For more info see https://getbootstrap.com/docs/5.0/forms/input-group/.
	/// </summary>
	public class Bootstrap5ControlGenerator : IInputControlGenerator
	{
		public TagBuilder Generate(LocalizationControlContext context)
		{
			Guard.Against.Null(context, nameof(context));

			// Create parent input group container
			var inputGroupTag = new TagBuilder(HtmlTagNames.Div);
			inputGroupTag.AddCssClass("input-group mb-3");

			// TODO: Need language service to get all available languages and to get current language
			inputGroupTag.InnerHtml
			.AppendHtml(@$"<button class='btn btn-outline-secondary dropdown-toggle' type='button' data-bs-toggle='dropdown' aria-expanded='false'>
				<span class='fi fi-{context.CurrentLanguage.FlagIcon} fis'></span>
			</button>")
			.AppendHtml(GenerateLanguageDropDown(context.Languages));

			for (int i = 0; i < context.Languages.Count; i++)
			{
				var lang = context.Languages[i];
				var propertyName = context.For.Name;
				var textDirection = lang.IsRtl ? "rtl" : "ltr";

				var inputValueId = $"{propertyName}_{nameof(TranslationModel.Translations)}_{i}_{nameof(TranslationEntryModel.Value)}";
				var inputValueName = $"{propertyName}.{nameof(TranslationModel.Translations)}[{i}].{nameof(TranslationEntryModel.Value)}";

				// Create input for lang code
				var inputLangCodeId = $"{propertyName}_{nameof(TranslationModel.Translations)}_{i}_{nameof(TranslationEntryModel.LangCode)}";
				var inputLangCodeName = $"{propertyName}.{nameof(TranslationModel.Translations)}[{i}].{nameof(TranslationEntryModel.LangCode)}";

				//var test = Generator.GenerateTextBox()

				var input = new TagBuilder(context.TagName);
				input.AddCssClass("form-control localized");
				//input.Attributes.Add("type", "text");
				input.Attributes.Add(key: TagAttributeNames.Id, inputValueId);
				input.Attributes.Add(TagAttributeNames.Name, inputValueName);
				input.Attributes.Add(TagAttributeNames.Lang, lang.CultureName);
				input.Attributes.Add(TagAttributeNames.Dir, textDirection);
				input.Attributes.Add(TagAttributeNames.Placeholder, lang.DisplayName);

				var isCurrentLanguage = lang.CultureName == context.CurrentLanguage.CultureName;

				if (context.TagName == "input")
				{
					input.Attributes.Add(TagAttributeNames.Type, isCurrentLanguage ? "text" : "hidden");
				}
				else
				{
					if (isCurrentLanguage)
					{
						input.Attributes.Add("hidden", "hidden");
					}
				}

				inputGroupTag.InnerHtml.AppendHtml(input);

				//Generate hidden input for language
				var langInput = new TagBuilder(HtmlTagNames.Input);
				langInput.Attributes.Add(key: "id", inputLangCodeId);
				langInput.Attributes.Add("name", inputLangCodeName);
				langInput.Attributes.Add("value", lang.CultureName);
				langInput.Attributes.Add("type", "hidden");

				inputGroupTag.InnerHtml.AppendHtml(langInput);
			}

			return inputGroupTag;
		}

		private static TagBuilder GenerateLanguageDropDown(IEnumerable<LanguageInfo> languages)
		{
			var ulTag = new TagBuilder(HtmlTagNames.Ul);
			ulTag.AddCssClass("dropdown-menu");
			ulTag.Attributes.Add("id", "dropdown-lang"); // Combine with parent guid ?

			// Append languages. No need for TagBuilder here ...
			foreach (var lang in languages)
			{
				ulTag.InnerHtml.AppendHtml(
				@$"<li lang='{lang.CultureName}'>
							<button type='button' class='dropdown-item'>
							<span class='fi fi-{lang.FlagIcon} fis me-2'></span>{lang.DisplayName}</button>
						</li>");
			}

			return ulTag;
		}
	}
}