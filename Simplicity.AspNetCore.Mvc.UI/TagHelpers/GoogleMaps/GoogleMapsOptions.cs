namespace Simplicity.AspNetCore.Mvc.UI.TagHelpers.GoogleMaps;

/// <summary>
/// Represents options used for google maps tag helper
/// </summary>
public class GoogleMapsOptions
{
    public GoogleMapsOptions()
    {
        ApiKeys = new Dictionary<string, string>();
    }

    /// <summary>
    /// Url placeholder for google maps initialization
    /// </summary>
    public string UrlPlaceholder { get; set; }

    /// <summary>
    /// Represents collection of api-name/api-key values
    /// </summary>
    public Dictionary<string, string> ApiKeys { get; set; }    
}