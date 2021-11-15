using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Localization;

namespace Cleaners.Web.Views.Shared
{
    /// <summary>
    /// Extension methods for <see cref="Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary"/> used inside Razor views/pages.
    /// This way we avoid dynamic properties with ViewBag and magic string used with ViewData
    /// </summary>
    public static class ViewDataDictionaryLayoutExtensions
    {
        /// <summary>
        /// Prefix for all view data keys
        /// </summary>
        private const string Prefix = "Layout_";

        private const string Title = Prefix + nameof(Title);

        /// <summary>
        /// Sets title for current page
        /// </summary>
        public static void SetPageTitle(this ViewDataDictionary viewData, string value)
        {
            Guard.Against.Null(viewData, nameof(viewData));

            if (!string.IsNullOrEmpty(value))
            {
                viewData[Title] = value;
            }
        }

        /// <summary>
        /// Sets title for current page
        /// </summary>
        public static void SetPageTitle(this ViewDataDictionary viewData, LocalizedString localizedString)
        {
            Guard.Against.Null(viewData, nameof(viewData));

            if (localizedString != null)
            {
                viewData[Title] = localizedString.ToString();
            }
        }

        /// <summary>
        /// Returns page title
        /// </summary>
        public static string GetPageTitle(this ViewDataDictionary viewData)
        {
            Guard.Against.Null(viewData, nameof(viewData));

            return viewData[Title] as string;
        }
    }
}