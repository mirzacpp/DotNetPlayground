using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Cleaners.Web.TagHelpers
{
    /// <summary>
    /// Tag helper for ajax forms
    /// </summary>
    /// <remarks>
    /// Tag helper will only be executed if form element contains "asp-ajax-enabled" attribute.
    /// </remarks>
    [HtmlTargetElement("form", Attributes = "asp-ajax-enabled")]
    public class AjaxFormTagHelper : FormTagHelper
    {
        public AjaxFormTagHelper(IHtmlGenerator generator)
            : base(generator)
        {
        }

        [HtmlAttributeName("asp-ajax-enabled")]
        public bool Enabled { get; set; }

        [HtmlAttributeName("asp-ajax-response-mode")]
        public AjaxResponseMode Mode { get; set; } = AjaxResponseMode.Replace;

        [HtmlAttributeName("asp-ajax-response-resubmit-element")]
        public bool ReSubmitTarget { get; set; }

        [HtmlAttributeName("asp-ajax-target-element")]
        public string TargetElementId { get; set; }

        [HtmlAttributeName("asp-ajax-response-update-element")]
        public string UpdateElementId { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            await base.ProcessAsync(context, output);

            if (Enabled)
            {
                output.Attributes.Add("data-ajax", Enabled);
                // Lower enum name for .js comparison
                output.Attributes.Add("data-ajax-response-mode", Mode.ToString().ToLowerInvariant());
                output.Attributes.Add("data-ajax-target", TargetElementId);
                output.Attributes.Add("data-ajax-response-resubmit-element", ReSubmitTarget);
                output.Attributes.Add("data-ajax-response-update-element", UpdateElementId);
            }
        }
    }

    public enum AjaxResponseMode
    {
        /// <summary>
        /// Default value
        /// </summary>
        Replace = 1,

        ReplaceWith,
        ReSubmit,
        Redirect,
    }
}