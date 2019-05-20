using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace Cleaners.Web.TagHelpers
{
    /// <summary>
    /// Allows conditions to be used as tag element
    /// </summary>
    public class IfTagHelper : TagHelper
    {
        public const string IncludeIfAttributeName = "include-if";
        public const string ExcludeIfAttributeName = "exclude-if";

        [HtmlAttributeName(IncludeIfAttributeName)]
        public bool Include { get; set; } = true;

        [HtmlAttributeName(ExcludeIfAttributeName)]
        public bool Exclude { get; set; } = false;

        public bool RenderTag { get; set; } = false;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            if (Exclude || !Include)
            {
                // Avoid rendering tag name, ie. <include-if></include-if> ...
                if (!RenderTag)
                {
                    output.TagName = null;
                }
                output.SuppressOutput();
            }
        }
    }
}