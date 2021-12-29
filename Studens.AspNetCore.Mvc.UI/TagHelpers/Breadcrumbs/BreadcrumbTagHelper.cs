using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Studens.AspNetCore.Mvc.UI.TagHelpers.Breadcrumbs;

/// <summary>
/// Represents breadcrumb tag helper
/// Credits to <see cref="https://github.com/abpframework/abp/blob/dev/framework/src/Volo.Abp.AspNetCore.Mvc.UI.Bootstrap/TagHelpers/Breadcrumb/AbpBreadcrumbTagHelperService.cs"/>
/// </summary>
public class BreadcrumbTagHelper : TagHelper
{
    #region Methods

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "nav";

        var items = InitializeBreadcrumbItemsCollection(context);

        await output.GetChildContentAsync();

        var builder = GetParentTagBuilder();

        PrepareItemsContent(items, builder);

        output.Content.SetHtmlContent(builder);
    }

    #endregion Methods

    #region Utils

    private TagBuilder GetParentTagBuilder()
    {
        var builder = new TagBuilder("ol");
        builder.AddCssClass("breadcrumb");
        return builder;
    }

    private List<BreadcrumbItem> InitializeBreadcrumbItemsCollection(TagHelperContext context)
    {
        var items = new List<BreadcrumbItem>();
        context.Items[Constants.BreadcrumbItemsKey] = items;
        return items;
    }

    private void PrepareItemsContent(List<BreadcrumbItem> list, TagBuilder listTagBuilder)
    {
        SetActiveItemIfThereIsNone(list);

        foreach (var breadcrumbItem in list)
        {
            var htmlPart = SetActiveClassIfActiveAndGetHtml(breadcrumbItem);

            listTagBuilder.InnerHtml.AppendHtml(htmlPart);
        }
    }

    private void SetActiveItemIfThereIsNone(List<BreadcrumbItem> list)
    {
        if (!list.Any(bci => bci.Active))
        {
            list.Last().Active = true;
        }
    }

    private string SetActiveClassIfActiveAndGetHtml(BreadcrumbItem item) =>
        item.Html.Replace(Constants.BreadcrumbItemActivePlaceholder, item.Active ? "active" : string.Empty);

    #endregion Utils
}