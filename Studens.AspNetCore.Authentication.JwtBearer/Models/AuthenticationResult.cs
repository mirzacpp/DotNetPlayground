namespace Studens.AspNetCore.Authentication.JwtBearer.Models;

public class AuthenticationResult
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public int ExpiresAtInSeconds { get; set; }
    public string UserId { get; set; }
}