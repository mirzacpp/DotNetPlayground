using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Simplicity.AspNetCore.Mvc.UI.TagHelpers;

/// Applies defined display template for specified model expression.
/// Source <see cref="https://github.com/DamianEdwards/TagHelperPack"/>
/// </summary>
[HtmlTargetElement("display", Attributes = "asp-for", TagStructure = TagStructure.WithoutEndTag)]
public class DisplayTagHelper : TagHelper
{
    private readonly IHtmlHelper _htmlHelper;

    public DisplayTagHelper(IHtmlHelper htmlHelper)
    {
        _htmlHelper = htmlHelper;
    }

    /// <summary>
    /// An expression to be evaluated against the current model.
    /// </summary>
    [HtmlAttributeName("asp-for")]
    public ModelExpression Model { get; set; }

    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext ViewContext { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        ((IViewContextAware)_htmlHelper).Contextualize(ViewContext);

        output.Content.SetHtmlContent(_htmlHelper.Display(Model));

        output.TagName = null;
    }
}