using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Studens.AspNetCore.Mvc.UI.TagHelpers.Localization
{
	/// <summary>
	/// Extends input tag helpers with multi-lingual support.
	/// </summary>
	[HtmlTargetElement("input", Attributes = "asp-localized")]
	public class InputLocalizedTagHelper : TagHelper
	{
		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			await base.ProcessAsync(context, output);
		}
	}
}