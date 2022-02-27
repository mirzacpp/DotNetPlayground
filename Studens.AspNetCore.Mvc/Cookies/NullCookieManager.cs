using Microsoft.AspNetCore.Http;

namespace Studens.AspNetCore.Mvc.Cookies;

/// <summary>
/// Cookie manager that does nothing.
/// </summary>
public class NullCookieManager : ICookieManager
{
    public static NullCookieManager Instance => new();

    public void Append<T>(string name, T value, CookieOptions? options = null) where T : IConvertible
    {
    }

    public void Delete(string name)
    {
    }

    public string? Get(string name) => null;

    public T? Get<T>(string name) where T : struct => null;
}