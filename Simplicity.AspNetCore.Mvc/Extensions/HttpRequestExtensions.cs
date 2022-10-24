using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Simplicity.AspNetCore.Mvc.Extensions;

/// <summary>
/// Extension methods for <see cref="HttpRequest"/>
/// </summary>
public static class HttpRequestExtensions
{
    public const string AjaxRequestHeader = "X-Requested-With";
    public const string AjaxRequestHeaderValue = "XMLHttpRequest";

    /// <summary>
    /// Determines if current request method is GET
    /// </summary>
    public static bool IsGet(this HttpRequest request) => HttpMethods.IsGet(request.Method);

    /// <summary>
    /// Determines if current request method is POST
    /// </summary>
    public static bool IsPost(this HttpRequest request) => HttpMethods.IsPost(request.Method);

    /// <summary>
    /// Determines if current request method is PUT
    /// </summary>
    public static bool IsPut(this HttpRequest request) => HttpMethods.IsPut(request.Method);

    /// <summary>
    /// Determines if current request method is DELETE
    /// </summary>
    public static bool IsDelete(this HttpRequest request) => HttpMethods.IsDelete(request.Method);

    /// <summary>
    /// Determines if current request method is PATCH
    /// </summary>
    public static bool IsPatch(this HttpRequest request) => HttpMethods.IsPatch(request.Method);

    /// <summary>
    /// Returns referer header for current request
    /// </summary>
    public static string? GetRefererHeader(this HttpRequest request) =>
        request.Headers[HeaderNames.Referer].FirstOrDefault();

    /// <summary>
    /// Determines if current request is an AJAX request    
    /// </summary>
    public static bool IsAjax(this HttpRequest request) =>
        request.Headers[AjaxRequestHeader] == AjaxRequestHeaderValue;
}