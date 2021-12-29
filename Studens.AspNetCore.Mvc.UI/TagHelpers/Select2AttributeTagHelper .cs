using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Studens.AspNetCore.Mvc.UI.TagHelpers.Extensions;

namespace Studens.AspNetCore.Mvc.UI.TagHelpers;

/// <summary>
/// Extends existing <see cref="Mvc.TagHelpers.SelectTagHelper"/> with select2 functionalities using data attributes
/// <see cref="https://jeesite.gitee.io/front/jquery-select2/4.0/options.htm#data-attributes"/>
/// </summary>
[HtmlTargetElement("select", Attributes = Select2AttributeName)]
[HtmlTargetElement("select", Attributes = Select2DisplayPlaceholderAttributeName)]
[HtmlTargetElement("select", Attributes = Select2PlaceholderTextAttributeName)]
[HtmlTargetElement("select", Attributes = Select2AllowClearAttributeName)]
[HtmlTargetElement("select", Attributes = Select2EnableTagsAttributeName)]
[HtmlTargetElement("select", Attributes = Select2AjaxUrlAttributeName)]
[HtmlTargetElement("select", Attributes = Select2LocalizationEnableAttributeName)]
[HtmlTargetElement("select", Attributes = Select2LocalizationCultureAttributeName)]
public class Select2AttributeTagHelper : SelectTagHelper
{
    #region Constants

    public const string Select2AttributeName = "select2-toggle";
    public const string Select2DisplayPlaceholderAttributeName = "select2-use-items-placeholder";
    public const string Select2PlaceholderTextAttributeName = "select2-placeholder-text";
    public const string Select2AllowClearAttributeName = "select2-allow-clear";
    public const string Select2EnableTagsAttributeName = "select2-tags";
    public const string Select2AjaxUrlAttributeName = "select2-ajax-url";
    public const string Select2AjaxEnableCacheAttributeName = "select2-ajax-cache";
    public const string Select2LocalizationEnableAttributeName = "select2-localization-auto";
    public const string Select2LocalizationCultureAttributeName = "select2-localization-culture";

    /// <summary>
    /// These data pairs are defined by select2 library
    /// </summary>
    public const string Select2PlaceholderDataAttributeName = "data-placeholder";

    public const string Select2AllowClearDataAttributeName = "data-allow-clear";
    public const string Select2TagsDataAttributeName = "data-tags";
    public const string Select2AjaxDataAttributeName = "data-ajax--url";
    public const string Select2AjaxCacheDataAttributeName = "data-ajax--cache";

    /// <summary>
    /// Select2 enables usage of existing HTML lang attribute
    /// </summary>
    public const string HtmlLanguageAttributeName = "lang";

    #endregion Constants

    private readonly IHttpContextAccessor _httpContextAccessor;

    public Select2AttributeTagHelper(IHtmlGenerator generator, IHttpContextAccessor httpContextAccessor)
        : base(generator)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    [HtmlAttributeName(Select2DisplayPlaceholderAttributeName)]
    public bool DisplayItemsPlaceholder { get; set; } = false;

    [HtmlAttributeName(Select2PlaceholderTextAttributeName)]
    public string PlaceholderText { get; set; }

    [HtmlAttributeName(Select2AllowClearAttributeName)]
    public bool AllowClear { get; set; }

    [HtmlAttributeName(Select2EnableTagsAttributeName)]
    public bool EnableTags { get; set; }

    [HtmlAttributeName(Select2AjaxUrlAttributeName)]
    public string AjaxUrl { get; set; }

    [HtmlAttributeName(Select2AjaxEnableCacheAttributeName)]
    public bool AjaxEnableCache { get; set; }

    /// <summary>
    /// Sets select2 list localization to clients current culture
    /// </summary>
    [HtmlAttributeName(Select2LocalizationEnableAttributeName)]
    public bool LocalizationEnableAuto { get; set; } = true;

    /// <summary>
    /// Sets select2 list localization to given culture if exists
    /// </summary>
    [HtmlAttributeName(Select2LocalizationCultureAttributeName)]
    public string LocalizationCulture { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        Guard.Against.Null(context, nameof(context));
        Guard.Against.Null(output, nameof(output));

        SetPlaceholderAttributes(output);

        output.Attributes.AddIf(AllowClear, Select2AllowClearDataAttributeName, true);
        output.Attributes.AddIf(EnableTags, Select2TagsDataAttributeName, true);

        SetLocalizationAttributes(output);
        SetAjaxAttributes(output);
    }

    private void SetPlaceholderAttributes(TagHelperOutput output)
    {
        if (output is null)
        {
            throw new ArgumentNullException(nameof(output));
        }

        // Fix for select2 responsiveness (https://github.com/select2/select2/issues/3278)
        output.Attributes.Add("data-width", "100%");

        // User can set placeholder text without select2-placeholder-show attribute set to true
        // Check only once and use where needed
        var hasAssignedPlaceholder = !string.IsNullOrEmpty(PlaceholderText);

        if (DisplayItemsPlaceholder || hasAssignedPlaceholder)
        {
            // Check if value is assigned via attribute, if not check if base.Items collection contains one
            string placeholderValue = PlaceholderText;

            if (!hasAssignedPlaceholder && Items != null && Items.Any())
            {
                // Take first item from collection and use it as placeholder
                // Add wrapper around SelectListItem with IsPlaceholder marker ?
                var placeholderItem = Items.First();

                placeholderValue = placeholderItem.Text;
            }

            output.Attributes.AddIf(!string.IsNullOrEmpty(placeholderValue), Select2PlaceholderDataAttributeName, placeholderValue);
        }
    }

    private void SetLocalizationAttributes(TagHelperOutput output)
    {
        if (LocalizationEnableAuto)
        {
            var currentCulture = _httpContextAccessor.HttpContext.Features.Get<IRequestCultureFeature>();

            output.Attributes.Add(HtmlLanguageAttributeName, currentCulture?.RequestCulture?.Culture.Name);
        }
        else if (!string.IsNullOrEmpty(LocalizationCulture))
        {
            output.Attributes.Add(HtmlLanguageAttributeName, LocalizationCulture);
        }
    }

    private void SetAjaxAttributes(TagHelperOutput output)
    {
        if (!string.IsNullOrEmpty(AjaxUrl))
        {
            output.Attributes.Add(Select2AjaxDataAttributeName, AjaxUrl);
            output.Attributes.AddIf(AjaxEnableCache, Select2AjaxCacheDataAttributeName, AjaxEnableCache);
        }
    }
}