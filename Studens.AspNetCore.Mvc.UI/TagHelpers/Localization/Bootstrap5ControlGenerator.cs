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

			// Prepened before input if language is ltr. Note that bootstrap 5 relies on actual html element position instead of prepend/append classes.
			if (!context.CurrentLanguage.IsRtl)
			{
				inputGroupTag.InnerHtml.AppendHtml(GenerateLanguageDropDown(context.Languages, context.CurrentLanguage));
			}

			for (int i = 0; i < context.Languages.Count; i++)
			{
				var lang = context.Languages[i];
				var propertyName = context.For.Name;
				var textDirection = lang.IsRtl ? TagAttributeValues.Rtl : TagAttributeValues.Ltr;

				var inputValueId = $"{propertyName}_{nameof(TranslationModel.Translations)}_{i}_{nameof(TranslationEntryModel.Value)}";
				var inputValueName = $"{propertyName}.{nameof(TranslationModel.Translations)}[{i}].{nameof(TranslationEntryModel.Value)}";

				// Create input for lang code
				var inputLangCodeId = $"{propertyName}_{nameof(TranslationModel.Translations)}_{i}_{nameof(TranslationEntryModel.LangCode)}";
				var inputLangCodeName = $"{propertyName}.{nameof(TranslationModel.Translations)}[{i}].{nameof(TranslationEntryModel.LangCode)}";

				var input = new TagBuilder(context.TagName)
				.WithId(inputValueId)
				.WithName(inputValueName)
				.WithClass("form-control localized")
				.WithAttribute(TagAttributeNames.Lang, lang.CultureName)
				.WithAttribute(TagAttributeNames.Dir, textDirection)
				.WithAttribute(TagAttributeNames.Placeholder, lang.DisplayName);

				var isCurrentLanguage = lang.CultureName == context.CurrentLanguage.CultureName;

				if (context.TagName == HtmlTagNames.Input)
				{
					input.Attributes.Add(TagAttributeNames.Type, isCurrentLanguage ? TagAttributeValues.Text : TagAttributeValues.Hidden);
				}
				else
				{
					if (!isCurrentLanguage)
					{
						input.Attributes.Add(TagAttributeValues.Hidden, TagAttributeValues.Hidden);
					}
				}

				inputGroupTag.InnerHtml.AppendHtml(input);

				//Generate hidden input for language
				var langInput = new TagBuilder(HtmlTagNames.Input)
				.AsInput(TagAttributeValues.Hidden)
				.WithId(inputLangCodeId)
				.WithName(inputLangCodeName)
				.WithValue(lang.CultureName);

				inputGroupTag.InnerHtml.AppendHtml(langInput);
			}

			//Append after input if language is rtl.
			if (context.CurrentLanguage.IsRtl)
			{
				inputGroupTag.InnerHtml.AppendHtml(GenerateLanguageDropDown(context.Languages, context.CurrentLanguage));
			}

			return inputGroupTag;
		}

		/// <summary>
		/// Generates HTML markup for language selection dropdown.
		/// </summary>
		/// <param name="languages">Active languages</param>
		/// <param name="language">Current language</param>
		private static TagBuilder GenerateLanguageDropDown(IEnumerable<LanguageInfo> languages, LanguageInfo language)
		{
			var btnGroupTag = new TagBuilder(HtmlTagNames.Div).WithClass("btn-group");

			btnGroupTag.InnerHtml.AppendHtml(@$"<button class='btn btn-outline-secondary dropdown-toggle' type='button' data-bs-toggle='dropdown' aria-expanded='false'>
				<span class='fi fi-{language.FlagIcon} fis'></span>
				</button>");

			var ulTag = new TagBuilder(HtmlTagNames.Ul)
			.WithClass("dropdown-menu")
			.WithId("dropdown-lang"); // Combine with parent guid ?

			// Right align dropdown if rtl
			if (language.IsRtl)
			{
				ulTag.WithClass("dropdown-menu-end");
			}

			// Append languages. No need for TagBuilder here ...
			foreach (var lang in languages)
			{
				ulTag.InnerHtml.AppendHtml(
				@$"<li lang='{lang.CultureName}' data-lang-flag-icon='{lang.FlagIcon}'>
							<button type='button' class='dropdown-item'>
								<span class='fi fi-{lang.FlagIcon} fis me-2'></span>{lang.DisplayName}
							</button>
						</li>");
			}

			btnGroupTag.InnerHtml.AppendHtml(ulTag);

			return btnGroupTag;
		}
	}
}