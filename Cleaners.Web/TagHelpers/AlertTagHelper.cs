using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Cleaners.Web.TagHelpers
{
    /// <summary>
    /// Generates bootstrap alert control
    /// </summary>
    [HtmlTargetElement("alert")]
    public class AlertTagHelper : TagHelper
    {
        [HtmlAttributeName("title")]
        public string Title { get; set; }

        [HtmlAttributeName("text")]
        public string Text { get; set; }

        [HtmlAttributeName("dismissable")]
        public bool IsDismissable { get; set; } = true;

        [HtmlAttributeName("type")]
        public AlertType AlertType { get; set; }

        /// <summary>
        /// Additional CSS classes separated by space character
        /// </summary>
        /// <remarks>
        /// CSS refers to main div wrapper
        /// </remarks>
        [HtmlAttributeName("class")]
        public string CssClasses { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var wrapper = new TagBuilder("div");
            wrapper.AddCssClass($"alert alert-{AlertType.ToString().ToLowerInvariant()} alert-dismissible");
            wrapper.AddCssClass(CssClasses);

            if (IsDismissable)
            {
                var button = new TagBuilder("button");
                button.InnerHtml.AppendHtml("&times;");
                button.AddCssClass("close");
                button.MergeAttribute("type", "button");
                button.MergeAttribute("data-dismiss", "alert");
                button.MergeAttribute("aria-hidden", "close");

                wrapper.InnerHtml.AppendHtml(button);
            }

            if (!string.IsNullOrEmpty(Title))
            {
                var title = new TagBuilder("h4");
                title.InnerHtml.Append(Title);

                wrapper.InnerHtml.AppendHtml(title);
            }

            wrapper.InnerHtml.AppendHtml(Text);

            // We don't want to render <alert> element
            output.TagName = null;
            output.Content.SetHtmlContent(wrapper);
        }
    }

    public enum AlertType
    {
        Success,
        Danger,
        Warning,
        Info
    }
}