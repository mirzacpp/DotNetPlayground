using Microsoft.AspNetCore.Http;

namespace Simplicity.AspNetCore.Mvc.Http.Cookies;

/// <summary>
/// Defines methods for cookies collection manipualtion.
/// </summary>
public interface ICookieManager
{
    /// <summary>
    /// Appends cookie to a current request.
    /// </summary>
    /// <typeparam name="T">Type of the value to append</typeparam>
    /// <param name="name">Cookie name</param>
    /// <param name="value">Cookie value</param>
    /// <param name="options">Cookie options</param>
    void Append<T>(string name, T value, CookieOptions? options = null) where T : IConvertible;

    /// <summary>
    /// Retrieves cookie value from a current request.
    /// </summary>
    /// <param name="name">Cookie name</param>
    /// <returns>Cookie value</returns>
    string? Get(string name);

    /// <summary>
    /// Retrieves cookie value from a current request as specified type.
    /// </summary>
    /// <typeparam name="T">Conversion type</typeparam>
    /// <param name="name">Cookie name</param>
    /// <returns>Cookie value</returns>
    T? Get<T>(string name) where T : struct;

    /// <summary>
    /// Deletes a cookie.
    /// </summary>
    /// <param name="name">Cookie name</param>
    void Delete(string name);
}