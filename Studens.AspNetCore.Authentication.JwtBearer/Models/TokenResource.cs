namespace Studens.AspNetCore.Authentication.JwtBearer.Models
{
    /// <summary>
    /// Represents a token
    /// </summary>
    public class TokenResource
    {
        public TokenResource(string token, int expiresInSeconds)
        {
            Token = token;
            ExpiresInSeconds = expiresInSeconds;
        }

        public string Token { get; set; }
        public int ExpiresInSeconds { get; set; }
    }
}