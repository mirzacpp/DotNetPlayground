using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Simplicity.AspNetCore.Mvc.UI.TagHelpers.Extensions;
using System.Text.Encodings.Web;

namespace Simplicity.AspNetCore.Mvc.UI.TagHelpers.Breadcrumbs;

/// <summary>
/// Represents breadcrumb item tag helper
/// Credits to <see cref="https://github.com/abpframework/abp/blob/dev/framework/src/Volo.Abp.AspNetCore.Mvc.UI.Bootstrap/TagHelpers/Breadcrumb/AbpBreadcrumbTagHelperService.cs"/>
/// </summary>
public class BreadcrumbItemTagHelper : TagHelper
{
    #region Fields

    private readonly HtmlEncoder _encoder;

    #endregion Fields

    #region Ctor

    public BreadcrumbItemTagHelper(HtmlEncoder encoder)
    {
        _encoder = encoder;
    }

    #endregion Ctor

    #region Props

    public string Href { get; set; }

    public string Title { get; set; }

    public bool Active { get; set; }

    #endregion Props

    #region Methods

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "li";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.AddClass("breadcrumb-item");
        // This value will be replaced with active item class if current item is marked as active.
        output.Attributes.AddClass(Constants.BreadcrumbItemActivePlaceholder);

        var items = context.GetValue<List<BreadcrumbItem>>(Constants.BreadcrumbItemsKey);

        output.Content.SetHtmlContent(GetInnerHtml(context, output));

        items.Add(new BreadcrumbItem
        {
            Html = output.Render(_encoder),
            Active = Active
        });

        // All content will be rendered by parent.
        output.SuppressOutput();
    }

    #endregion Methods

    #region Utils

    private string GetInnerHtml(TagHelperContext context, TagHelperOutput output)
    {
        if (string.IsNullOrWhiteSpace(Href))
        {
            output.Attributes.Add("aria-current", "page");
            return _encoder.Encode(Title);
        }

        var link = new TagBuilder("a");
        link.Attributes.Add("href", Href);
        link.InnerHtml.AppendHtml(Title);

        return link.ToHtmlString();
    }

    #endregion Utils
}