using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Simplicity.AspNetCore.Mvc.UI.TagHelpers;

/// <summary>
/// Tag helper representation of <see cref="IHtmlHelper.AntiForgeryToken()"/>
/// </summary>
[HtmlTargetElement("antiforgery-token", TagStructure = TagStructure.WithoutEndTag)]
public class AntiforgeryTokenTagHelper : TagHelper
{
    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext ViewContext { get; set; }

    public IHtmlGenerator HtmlGenerator { get; }

    public AntiforgeryTokenTagHelper(IHtmlGenerator htmlGenerator)
    {
        HtmlGenerator = htmlGenerator;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        Guard.Against.Null(context, nameof(context));
        Guard.Against.Null(output, nameof(output));

        // Cleans output
        output.SuppressOutput();

        // Instanciate antiforgery element using generator and current view context
        var antiforgeryTag = HtmlGenerator.GenerateAntiforgery(ViewContext);

        if (antiforgeryTag != null)
        {
            output.Content.SetHtmlContent(antiforgeryTag);
        }
    }
}