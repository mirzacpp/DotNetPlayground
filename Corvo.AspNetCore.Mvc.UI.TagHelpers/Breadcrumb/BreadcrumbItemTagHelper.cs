using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;

namespace Corvo.AspNetCore.Mvc.UI.TagHelpers.Breadcrumb
{
    public class BreadcrumbItemTagHelper : TagHelper
    {
        public string Title { get; set; }
        public string Href { get; set; }
        public bool Active { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "li";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("class", "breadcrumb-item");

            var innerHtml = string.IsNullOrWhiteSpace(Href) ?
                Title :
                $@"<a href=""{ Href }"">{Title}</a>";

            var items = (List<BreadcrumbItem>)context.Items["breadcrumbitems"] ?? new List<BreadcrumbItem>();

            // Set generated content
            output.Content.SetHtmlContent(innerHtml);

            items.Add(new BreadcrumbItem
            {
                Html = innerHtml,
                Active = Active
            });

            // Avoid content rendering
            output.SuppressOutput();
        }
    }
}