using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Http;

namespace Simplicity.AspNetCore.Mvc.Http.Cookies;

/// <summary>
/// Default cookie manager implementation
/// TODO: Add service collection extension methods for registration
/// </summary>
public class CookieManager : ICookieManager
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private HttpContext? _context;

    /// <summary>
    /// Default cookie options
    /// TODO: Make configurable at startup
    /// </summary>
    private static readonly CookieOptions _defaultOptions = new()
    {
        HttpOnly = true,
        IsEssential = false,
        Secure = true,
        SameSite = SameSiteMode.Strict,
        Expires = DateTime.UtcNow.AddMinutes(30)
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
            return context ?? throw new InvalidOperationException("HttpContext must not be null.");
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
           value.As<string>(),
           options ?? _defaultOptions);
    }

    public void Delete(string name) => Context.Response.Cookies.Delete(name);

    public string? Get(string name) => GetValue(name);

    public T? Get<T>(string name) where T : struct => GetValue(name)?.To<T>();

    private string? GetValue(string name) => Context.Request.Cookies[name];
}