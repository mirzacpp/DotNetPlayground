using Microsoft.AspNetCore.Http;

namespace Studens.AspNetCore.Mvc.Extensions;

/// <summary>
/// Extension methods for <see cref="HttpRequest"/>
/// </summary>
public static class HttpRequestExtensions
{
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
}
