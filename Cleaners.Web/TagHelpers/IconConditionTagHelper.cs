using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Cleaners.Web.TagHelpers
{
    /// <summary>
    /// Display true or false with icon for given condition
    /// </summary>
    /// <remarks>
    /// Enable attributes overriding?
    /// </remarks>
    [HtmlTargetElement("icon-condition")]
    public class IconConditionTagHelper : TagHelper
    {
        private static readonly string _successClass = "text-success";
        private static readonly string _errorClass = "text-error";

        [HtmlAttributeName("status")]
        public bool Status { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var i = new TagBuilder("i");

            i.AddCssClass(Status ? _successClass : _errorClass);
            i.AddCssClass(Status ? "fa fa-check" : "fa fa-times");

            output.Content.AppendHtml(i);
        }
    }
}