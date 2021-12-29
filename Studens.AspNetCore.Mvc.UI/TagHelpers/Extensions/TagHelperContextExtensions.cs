using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Studens.AspNetCore.Mvc.UI.TagHelpers.Extensions;

internal static class TagHelperContextExtensions
{
    public static T GetValue<T>(this TagHelperContext context, string key)
    {
        if (!context.Items.ContainsKey(key))
        {
            return default;
        }

        return (T)context.Items[key];
    }
}