using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Cleaners.Web.TagHelpers.Nav
{
    /// <summary>
    /// Custom component that will inject specified markup to <nav></nav> element
    /// </summary>    
    public class NavTagHelperComponent : TagHelperComponent
    {
        public const string ThemifyAttribute = "themify";

        public override int Order => 1;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (string.Equals(context.TagName, "nav", StringComparison.OrdinalIgnoreCase) && output.Attributes.ContainsName(ThemifyAttribute))
            {
                // Append theme class                
                output.AddClass("bg-blue-600", HtmlEncoder.Default);                
            }
        }
    }
}