using Microsoft.AspNetCore.Razor.TagHelpers;
using Simplicity.AspNetCore.Mvc.UI.TagHelpers.Extensions;
using System.Globalization;

namespace Simplicity.AspNetCore.Mvc.UI.TagHelpers;

/// <summary>
/// Extends <see cref="FormTagHelper"/> and <see cref="AnchorTagHelper"/> elements with ajax data-* attributes
/// Mimics BeginForm from <see cref="https://github.com/mono/aspnetwebstack/blob/master/src/System.Web.Mvc/Ajax/AjaxExtensions.cs"/>
/// In sync with <see cref="https://github.com/aspnet/jquery-ajax-unobtrusive/blob/master/src/jquery.unobtrusive-ajax.js"/>
/// </summary>
[HtmlTargetElement("a", Attributes = AjaxAttributeName)]
[HtmlTargetElement("form", Attributes = AjaxAttributeName)]
public class AjaxAttributeTagHelper : TagHelper
{
    #region Constants

    public const string AjaxAttributeName = "ajax";
    public const string AjaxConfirmAttributeName = "ajax-confirm";
    public const string AjaxMethodAttributeName = "ajax-method";
    public const string AjaxUpdateElementAttributeName = "ajax-update";
    public const string AjaxModeAttributeName = "ajax-mode";
    public const string AjaxLoadingElementAttributeName = "ajax-loading";
    public const string AjaxLoadingElemenDurationtAttributeName = "ajax-loading-duration";
    public const string AjaxSucessAttributeName = "ajax-success";
    public const string AjaxFailureAttributeName = "ajax-failure";
    public const string AjaxBeginAttributeName = "ajax-begin";
    public const string AjaxCompleteAttributeName = "ajax-complete";
    public const string AjaxUrlAttributeName = "ajax-url";
    public const string AjaxResubmitElementName = "ajax-resubmit-element";

    #endregion Constants

    /// <summary>
    /// Must be set to true to activate unobtrusive Ajax on the target element.
    /// Has to be present because we are only extending existing form tag helper ...
    /// </summary>
    [HtmlAttributeName(AjaxAttributeName)]
    public bool Enabled { get; set; }

    /// <summary>
    /// Gets or sets the message to display in a confirmation window before a request is submitted.
    /// </summary>
    [HtmlAttributeName(AjaxConfirmAttributeName)]
    public string? ConfirmMessage { get; set; }

    /// <summary>
    /// Used to resubmit form with given identifier
    /// </summary>
    [HtmlAttributeName(AjaxResubmitElementName)]
    public string? ResubmitElement { get; set; }

    /// <summary>
    /// Gets or sets the HTTP request method ("Get" or "Post").
    /// </summary>
    [HtmlAttributeName(AjaxMethodAttributeName)]
    public string? FormMethod { get; set; }

    /// <summary>
    /// Gets or sets the ID of the DOM element to update by using the response from the server.
    /// </summary>
    [HtmlAttributeName(AjaxUpdateElementAttributeName)]
    public string UpdateElementId { get; set; }

    /// <summary>
    /// Gets or sets the id attribute of an HTML element that is displayed while the Ajax function is loading.
    /// </summary>
    [HtmlAttributeName(AjaxLoadingElementAttributeName)]
    public string? LoadingElementId { get; set; }

    /// <summary>
    /// Gets or sets a value, in milliseconds, that controls the duration of the animation when showing or hiding the loading element.
    /// </summary>
    [HtmlAttributeName(AjaxLoadingElemenDurationtAttributeName)]
    public int LoadingElementDuration { get; set; }

    /// <summary>
    /// Gets or sets the mode that specifies how to insert the response into the target DOM element. Valid values are before, after and replace. Default is replace
    /// </summary>
    [HtmlAttributeName(AjaxModeAttributeName)]
    public InsertionMode InsertionMode { get; set; } = InsertionMode.Replace;

    /// <summary>
    /// Gets or sets the JavaScript function to call after the page is successfully updated.
    /// </summary>
    [HtmlAttributeName(AjaxSucessAttributeName)]
    public string? OnSuccessMethod { get; set; }

    /// <summary>
    /// Gets or sets the JavaScript function to call if the page update fails.
    /// </summary>
    [HtmlAttributeName(AjaxFailureAttributeName)]
    public string? OnFailureMethod { get; set; }

    /// <summary>
    /// Gets or sets the JavaScript function to call when response data has been instantiated but before the page is updated.
    /// </summary>
    [HtmlAttributeName(AjaxCompleteAttributeName)]
    public string? OnCompleteMethod { get; set; }

    /// <summary>
    /// Gets or sets the name of the JavaScript function to call immediately before the page is updated.
    /// </summary>
    [HtmlAttributeName(AjaxBeginAttributeName)]
    public string? OnBeginMethod { get; set; }

    /// <summary>
    /// Gets or sets the URL to make the request to.
    /// </summary>
    [HtmlAttributeName(AjaxBeginAttributeName)]
    public string? Url { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        Guard.Against.Null(context, nameof(context));
        Guard.Against.Null(output, nameof(output));

        await base.ProcessAsync(context, output);

        // Ignore all attributes if data-ajax is set to false
        if (Enabled)
        {
            // Use hardcoded value to avoid string operations and culture problems("true/false" values instead of "True/False" etc.)
            output.Attributes.Add(AjaxDefaults.DataAjaxAttributeName, "true");
            output.Attributes.AddIf(!string.IsNullOrEmpty(ConfirmMessage), AjaxDefaults.DataAjaxConfirmMessageAttributeName, ConfirmMessage);
            output.Attributes.AddIf(!string.IsNullOrEmpty(FormMethod), AjaxDefaults.DataAjaxFormMethodAttributeName, FormMethod);
            output.Attributes.AddIf(!string.IsNullOrEmpty(OnSuccessMethod), AjaxDefaults.DataAjaxOnSuccessMethodAttributeName, OnSuccessMethod);
            output.Attributes.AddIf(!string.IsNullOrEmpty(OnFailureMethod), AjaxDefaults.DataAjaxOnFailureMethodAttributeName, OnFailureMethod);
            output.Attributes.AddIf(!string.IsNullOrEmpty(OnBeginMethod), AjaxDefaults.DataAjaxOnBeginMethodAttributeName, OnBeginMethod);
            output.Attributes.AddIf(!string.IsNullOrEmpty(OnCompleteMethod), AjaxDefaults.DataAjaxOnCompleteMethodAttributeName, OnCompleteMethod);
            output.Attributes.AddIf(!string.IsNullOrEmpty(Url), AjaxDefaults.DataAjaxUrlAttributeName, Url);

            if (!string.IsNullOrEmpty(ResubmitElement))
            {
                output.Attributes.Add(AjaxDefaults.DataAjaxResubmitElementAttributeName, ProcessElementId(ResubmitElement));
            }

            if (!string.IsNullOrEmpty(UpdateElementId))
            {
                output.Attributes.Add(AjaxDefaults.DataAjaxUpdateElementIdAttributeName, ProcessElementId(UpdateElementId));
                // Append insertion mode only if update element id is present
                output.Attributes.Add(AjaxDefaults.DataAjaxInsertionModeAttributeName, InsertionMode.ToInsertionModeUnobtrusive());
            }

            if (!string.IsNullOrEmpty(LoadingElementId))
            {
                output.Attributes.Add(AjaxDefaults.DataAjaxLoadingElementIdAttributeName, ProcessElementId(LoadingElementId));
                output.Attributes.AddIf(LoadingElementDuration > 0, AjaxDefaults.DataAjaxLoadingElementDurationAttributeName, LoadingElementDuration);
            }
        }
    }

    /// <summary>
    /// Checks if given value is valid jquery selector
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private string ProcessElementId(string value)
        => value.StartsWith("#") ? value : "#" + value;
}

/// <summary>
/// Used to determine where html content should be rendered
/// </summary>
public enum InsertionMode
{
    Replace = 0,
    InsertBefore = 1,
    InsertAfter = 2
}

/// <summary>
/// Extension methods for <see cref="InsertionMode"/> used only inside <see cref="AjaxFormAttributeTagHelper"/>
/// </summary>
internal static class InsertionModeExtensions
{
    /// <summary>
    /// Returns proper insertion value based on enum <paramref name="value"/>
    /// </summary>
    /// <param name="value">Insertion mode</param>
    /// <returns></returns>
    public static string ToInsertionModeUnobtrusive(this InsertionMode value)
    {
        return value switch
        {
            InsertionMode.Replace => "replace",
            InsertionMode.InsertBefore => "before",
            InsertionMode.InsertAfter => "after",
            // Default value will result with jquerys .html method
            _ => ((int)value).ToString(CultureInfo.InvariantCulture),
        };
    }
}