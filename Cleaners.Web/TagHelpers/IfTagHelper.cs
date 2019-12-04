using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Cleaners.Web.TagHelpers
{
    /// <summary>
    /// Allows conditions to be used as tag element
    /// </summary>
    [HtmlTargetElement("if", Attributes = IncludeIfAttributeName)]
    [HtmlTargetElement("if", Attributes = ExcludeIfAttributeName)]
    public class IfTagHelper : TagHelper
    {
        public const string IncludeIfAttributeName = "include-if";
        public const string ExcludeIfAttributeName = "exclude-if";

        [HtmlAttributeName(IncludeIfAttributeName)]
        public bool Include { get; set; } = true;

        [HtmlAttributeName(ExcludeIfAttributeName)]
        public bool Exclude { get; set; } = false;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            Guard.Against.Null(context, nameof(context));
            Guard.Against.Null(output, nameof(output));

            if (Exclude || !Include)
            {
                // Avoid rendering tag name, ie. <include-if></include-if> ...
                output.TagName = null;
                output.SuppressOutput();
            }
        }
    }
}