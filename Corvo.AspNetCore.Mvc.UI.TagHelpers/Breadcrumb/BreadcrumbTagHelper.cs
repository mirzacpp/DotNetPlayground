using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Corvo.AspNetCore.Mvc.UI.TagHelpers.Breadcrumb
{
    /// <summary>
    /// Tag helper for breadcrumb generation
    /// </summary>
    [HtmlTargetElement(TagHelperConstants.Breadcrumb.Name)]
    [RestrictChildren("breadcrumb-item")]
    public class BreadcrumbTagHelper : TagHelper
    {
        public string Title { get; set; }
        public string Href { get; set; }
        public bool Active { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            
        }
    }
}