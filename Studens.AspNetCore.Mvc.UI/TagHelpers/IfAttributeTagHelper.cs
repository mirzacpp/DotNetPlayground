using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Studens.AspNetCore.Mvc.UI.TagHelpers;

/// <summary>
/// Allows conditions to be used as attributes inside tag elements
/// </summary>
[HtmlTargetElement(Attributes = IncludeIfAttributeName)]
[HtmlTargetElement(Attributes = ExcludeIfAttributeName)]
public class IfAttributeTagHelper : TagHelper
{
    public const string IncludeIfAttributeName = "include-if";
    public const string ExcludeIfAttributeName = "exclude-if";

    /// <summary>
    /// Set order to negative because lower orders are executed first
    /// </summary>
    public override int Order => -1000;

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