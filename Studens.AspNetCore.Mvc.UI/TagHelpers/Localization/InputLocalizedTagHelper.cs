using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Studens.AspNetCore.Mvc.UI.TagHelpers.Localization
{
	/// <summary>
	/// Extends input tag helpers with multi-lingual support.
	/// </summary>
	[HtmlTargetElement("input", Attributes = "asp-localized")]
	public class InputLocalizedTagHelper : InputTagHelper
	{
		public InputLocalizedTagHelper(IHtmlGenerator generator)
		: base(generator)
		{
		}

		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			await base.ProcessAsync(context, output);			

			output.Attributes.Add("test", "test");
		}
	}
}