using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
using System.ComponentModel;

namespace Cleaners.Web.TagHelpers.Nav
{
    /// <summary>
    /// Custom tag helper component for targeting <nav></nav> HTML element
    /// </summary>
    [HtmlTargetElement("nav")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class NavTagHelperComponentTagHelper : TagHelperComponentTagHelper
    {
        public NavTagHelperComponentTagHelper(ITagHelperComponentManager manager, ILoggerFactory loggerFactory) : base(manager, loggerFactory)
        {
        }
    }
}