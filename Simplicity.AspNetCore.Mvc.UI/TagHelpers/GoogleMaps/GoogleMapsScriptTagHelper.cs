using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Simplicity.AspNetCore.Mvc.UI.TagHelpers.GoogleMaps;

[HtmlTargetElement("google-maps-script", TagStructure = TagStructure.NormalOrSelfClosing)]
public class GoogleMapsScriptTagHelper : TagHelper
{
    private readonly GoogleMapsOptions _options;

    public GoogleMapsScriptTagHelper(IOptions<GoogleMapsOptions> optionsAccessor)
    {
        _options = optionsAccessor.Value;
    }

    private IDictionary<string, string> _queryValues;

    [HtmlAttributeName("google-maps-query-data", DictionaryAttributePrefix = "google-maps-query-")]
    public IDictionary<string, string> QueryValues
    {
        get
        {
            if (_queryValues == null)
            {
                _queryValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            }

            return _queryValues;
        }
        set
        {
            _queryValues = value;
        }
    }

    /// <summary>
    /// Api key name
    /// </summary>
    [HtmlAttributeName("api-key-name")]
    public string ApiKeyName { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        Guard.Against.Null(context, nameof(context));
        Guard.Against.Null(output, nameof(output));

        var apiKey = _options.ApiKeys.GetValueOrDefault(ApiKeyName);

        if (string.IsNullOrEmpty(apiKey))
        {
            throw new InvalidOperationException("Specified api key could not be found.");
        }

        var uri = string.Format(_options.UrlPlaceholder, apiKey);

        // Append additional query string parameters, such as callback etc.
        if (_queryValues != null && _queryValues.Count > 0)
        {
            foreach (var param in _queryValues)
            {
                uri += $"&{param.Key}={param.Value}";
            }
        }

        output.Content.AppendHtml("<script src=" + uri + "></script>");
        // Don't output original tag
        output.TagName = null;
    }
}