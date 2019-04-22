using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Cleaners.Web.TagHelpers
{
    /// <summary>
    /// Allows conditions to be used in combination with HTML tags
    /// </summary>
    [HtmlTargetElement(Attributes = AspConditionAttributeName)]
    public class ConditionalTagHelper : TagHelper
    {
        public const string AspConditionAttributeName = "asp-if";

        [HtmlAttributeName(AspConditionAttributeName)]
        public bool Condition { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!Condition)
            {
                // If condition is false, use SuppressOutput method to avoid markup rendering.
                output.SuppressOutput();
            }
        }
    }
}