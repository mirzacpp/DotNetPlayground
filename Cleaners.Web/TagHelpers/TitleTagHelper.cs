using Cleaners.Web.Infrastructure.AppSettings;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Threading.Tasks;

namespace Cleaners.Web.TagHelpers
{
    /// <summary>
    /// Title tag helper
    /// </summary>
    [HtmlTargetElement("title")]
    public class TitleTagHelper : TagHelper
    {
        private readonly AppInfoConfig _appInfo;

        public TitleTagHelper(AppInfoConfig appInfo)
        {
            _appInfo = appInfo ?? throw new ArgumentNullException(nameof(appInfo));
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            // Use NullHtmlEncoder to avoid content encoding
            var content = await output.GetChildContentAsync(NullHtmlEncoder.Default);

            var title = string.IsNullOrEmpty(content.GetContent()) ?
                _appInfo.FullName :
                $"{_appInfo.FullName} {_appInfo.TitleDelimiter} {content.GetContent()}";

            output.Content.SetContent(title);
        }
    }
}