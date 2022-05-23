using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;

namespace Studens.AspNetCore.Mvc.UI.TagHelpers;

/// <summary>
/// Renders JavaScript necessary to load data via AJAX
/// </summary>
[HtmlTargetElement(AjaxLoaderAttributeName)]
public class AjaxLoaderTagHelper : TagHelper
{
    private const string AjaxLoaderAttributeName = "ajax-loader";
    private const string AjaxLoadingElementAttributeName = "ajax-loading";
    private const string AjaxLoadingElemenDurationtAttributeName = "ajax-loading-duration";
    private const string AjaxUpdateElementAttributeName = "ajax-update";
    private const string AjaxControllerAttributeName = "ajax-controller";
    private const string AjaxActionAttributeName = "ajax-action";
    private const string AjaxRouteAttributeName = "ajax-route";

    private readonly IUrlHelperFactory _urlHelperFactory;
    private IDictionary<string, string> _routeValues;

    public AjaxLoaderTagHelper(IUrlHelperFactory urlHelperFactory)
    {
        _urlHelperFactory = urlHelperFactory;
    }

    [HtmlAttributeName(AjaxActionAttributeName)]    
    public string Action { get; set; }

    [HtmlAttributeName(AjaxControllerAttributeName)]
    public string Controller { get; set; }

    [HtmlAttributeName(AjaxRouteAttributeName)]
    public string Route { get; set; }

    /// <summary>
    /// Gets or sets the ID of the DOM element to update by using the response from the server.
    /// </summary>
    [HtmlAttributeName(AjaxUpdateElementAttributeName)]
    public string UpdateElementId { get; set; }

    /// <summary>
    /// Gets or sets the id attribute of an HTML element that is displayed while the Ajax function is loading.
    /// </summary>
    [HtmlAttributeName(AjaxLoadingElementAttributeName)]
    public string LoadingElementId { get; set; }

    /// <summary>
    /// Gets or sets a value, in milliseconds, that controls the duration of the animation when showing or hiding the loading element.
    /// </summary>
    [HtmlAttributeName(AjaxLoadingElemenDurationtAttributeName)]
    public int LoadingElementDuration { get; set; }

    [HtmlAttributeName("ajax-all-route-data", DictionaryAttributePrefix = "ajax-route-")]
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

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        Guard.Against.Null(context, nameof(context));
        Guard.Against.Null(output, nameof(output));

        output.TagName = "div";
        // TODO: Generate random element id if not specified or throw exception?        
        var elementId = output.Attributes["id"]?.Value.ToString();

        // If ajax-update/data-ajax-update is not present, just assign it with element id value
        UpdateElementId ??= elementId;
        UpdateElementId = ProcessElementId(UpdateElementId);

        output.Attributes.Add(AjaxDefaults.DataAjaxUpdateElementIdAttributeName, UpdateElementId);
        output.Content.AppendHtml(GenerateScriptTag(GenerateUri(), UpdateElementId));
    }

    /// <summary>
    /// Checks if given value is valid jquery selector
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private string ProcessElementId(string value)
        => value.StartsWith("#") ? value : "#" + value;

    private string GenerateScriptTag(string uri, string elementId)
    {
        // Keep in sync with unobtrusive-ajax.js script
        var content = $@"<script>
                                $(function() {{
                                    $(""{elementId}"").asyncRequest({{
                                        url: ""{uri}"",
                                        type: ""GET"",
                                        data:[]
                                    }});
                                }});
                            </script>";

        return content;
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
}