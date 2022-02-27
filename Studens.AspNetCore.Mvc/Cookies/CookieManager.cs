using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Http;

namespace Studens.AspNetCore.Mvc.Cookies;

/// <summary>
/// Default cookie manager implementation
/// </summary>
public class CookieManager : ICookieManager
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private HttpContext? _context;

    /// <summary>
    /// Default cookie options
    /// TODO: Make configurable at startup
    /// </summary>
    private static readonly CookieOptions _options = new()
    {
        HttpOnly = true,
        IsEssential = false,
        Secure = true,
        SameSite = SameSiteMode.Strict,
        Expires = DateTimeOffset.UtcNow.AddMinutes(30)
    };

    public CookieManager(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private HttpContext Context
    {
        get
        {
            var context = _context ?? _httpContextAccessor?.HttpContext;
            if (context == null)
            {
                throw new InvalidOperationException("HttpContext must not be null.");
            }
            return context;
        }
        set
        {
            _context = value;
        }
    }

    public void Append<T>(string name, T value, CookieOptions? options = null) where T : IConvertible
    {
        Guard.Against.NullOrEmpty(name, nameof(name));

        Context.Response.Cookies.Append(
           name,
           value.To<string>(),
           options ?? _options);
    }

    public void Delete(string name) => Context.Response.Cookies.Delete(name);

    public string? Get(string name) => GetValue(name);

    public T? Get<T>(string name) where T : struct => GetValue(name)?.To<T>();

    private string? GetValue(string name) => Context.Request.Cookies[name];
}