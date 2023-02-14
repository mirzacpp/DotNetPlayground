using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Simplicity.Templates.WebUI.Features.Shared;

public static partial class ViewDataDictionaryExtensions
{
    private const string KeyPrefix = "Layout.";
    private const string PageTitleKey = KeyPrefix + "Title";

    public static void SetPageTitle(this ViewDataDictionary viewData, string title) =>
        viewData[PageTitleKey] = title;

    public static string? GetPageTitle(this ViewDataDictionary viewData) =>
        viewData[PageTitleKey] as string;
}