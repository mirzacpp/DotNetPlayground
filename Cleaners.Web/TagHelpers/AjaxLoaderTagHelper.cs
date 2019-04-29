using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;

namespace Cleaners.Web.TagHelpers
{
    /// <summary>
    /// Enables ajax loading combining JavaScript and TagHelpers
    /// </summary>
    [HtmlTargetElement(AspAjaxLoaderAttributeName)]
    public class AjaxLoaderTagHelper : TagHelper
    {
        #region Constants

        // This should be synced with ajax.js code
        private const string JavaScriptCodePlaceholder = @"getData('{0}', '{1}', {2})";

        private const string AspAjaxLoaderAttributeName = "ajax-loader";

        private const string DefaultIdName = "tableData";

        #endregion Constants

        #region Fields

        private readonly IUrlHelperFactory _urlHelperFactory;
        private IDictionary<string, string> _routeValues;

        #endregion Fields

        #region Constructor

        public AjaxLoaderTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        #endregion Constructor

        #region Properties

        [HtmlAttributeName("asp-ajax-action")]
        public string Action { get; set; }

        [HtmlAttributeName("asp-ajax-controller")]
        public string Controller { get; set; }

        [HtmlAttributeName("asp-ajax-route")]
        public string Route { get; set; }

        [HtmlAttributeName("asp-ajax-blockui")]
        public bool BlockUi { get; set; }

        [HtmlAttributeName("asp-ajax-all-route-data", DictionaryAttributePrefix = "asp-ajax-route-")]
        public IDictionary<string, string> RouteValues
        {
            get
            {
                if (_routeValues == null)
                {
                    _routeValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                }

                return _routeValues;
            }
            set
            {
                _routeValues = value;
            }
        }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        #endregion Properties

        #region Methods

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var div = new TagBuilder("div");

            var targetUri = GenerateUri();
            var targetId = output.Attributes["id"]?.Value.ToString() ?? DefaultIdName;

            div.MergeAttribute("id", targetId);

            div.InnerHtml.AppendHtml(GenerateScriptTag(targetUri, targetId));

            div.AddCssClass(output.Attributes["class"]?.Value.ToString());

            // We don't want to render <ajax-load></ajax-load> element around div
            output.TagName = null;
            output.Content.AppendHtml(div);
        }

        private TagBuilder GenerateScriptTag(string uri, string targetElement)
        {
            var script = new TagBuilder("script");
            var content = string.Format(
                JavaScriptCodePlaceholder,
                uri,
                targetElement,
                BlockUi.ToString().ToLowerInvariant());

            // Use AppendHtml to avoid content encoding
            script.InnerHtml.AppendHtml(content);

            return script;
        }

        private string GenerateUri()
        {
            if (string.IsNullOrEmpty(Action) && string.IsNullOrEmpty(Controller) && string.IsNullOrEmpty(Route))
            {
                throw new InvalidOperationException("Cannot determine action/href attribute based on asp-ajax-route, asp-ajax-controller and asp-ajax-action values.");
            }

            if (!string.IsNullOrEmpty(Route) && (!string.IsNullOrEmpty(Action) || !string.IsNullOrEmpty(Controller)))
            {
                throw new InvalidOperationException("Attribute asp-ajax-route cannot be used in combination with asp-ajax-controller and asp-ajax-action attributes.");
            }

            RouteValueDictionary routeValues = null;
            if (_routeValues != null && _routeValues.Count > 0)
            {
                routeValues = new RouteValueDictionary(_routeValues);
            }

            var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);

            if (urlHelper == null)
            {
                throw new NullReferenceException(nameof(urlHelper));
            }

            if (!string.IsNullOrEmpty(Route))
            {
                return urlHelper.RouteUrl(Route, routeValues);
            }

            return urlHelper.Action(Action, Controller, routeValues);
        }

        #endregion Methods
    }
}