using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Cleaners.Web.TagHelpers
{
    public class MetaTagHelperComponent : TagHelperComponent
    {
        public override int Order => 1;

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context.TagName == "head")
            {
                output.PostContent.AppendHtml(@"<meta name=""description"" content=""This repository demostrates how to add dynamic CSS and JS scripts to your razor pages with ASP.NET Core Tag Helper Components. - fiyazbinhasan / TagHelperComponentSample"">");
            }

            return Task.CompletedTask;
        }
    }
}