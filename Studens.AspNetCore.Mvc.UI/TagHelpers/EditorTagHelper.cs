using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Studens.AspNetCore.Mvc.UI.TagHelpers;

/// <summary>
/// Applies defined editor template for specified model expression.
/// Source <see cref="https://github.com/DamianEdwards/TagHelperPack"/>
/// </summary>
[HtmlTargetElement("editor", Attributes = "asp-for", TagStructure = TagStructure.WithoutEndTag)]
public class EditorTagHelper : TagHelper
{
    private readonly IHtmlHelper _htmlHelper;

    public EditorTagHelper(IHtmlHelper htmlHelper)
    {
        _htmlHelper = htmlHelper;
    }

    /// <summary>
    /// An expression to be evaluated against the current model.
    /// </summary>
    [HtmlAttributeName("asp-for")]
    public ModelExpression For { get; set; }

    [HtmlAttributeName("asp-additional-view-data")]
    public object AdditionalViewData { get; set; }

    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext ViewContext { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        ((IViewContextAware)_htmlHelper).Contextualize(ViewContext);

        output.Content.SetHtmlContent(_htmlHelper.Editor(For, AdditionalViewData));

        output.TagName = null;
    }
}