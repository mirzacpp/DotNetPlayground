namespace Simplicity.AspNetCore.Authentication.JwtBearer.Models
{
    /// <summary>
    /// Represents a token resource
    /// </summary>
    public class TokenResource
    {
        public TokenResource(string accessTokenValue, string refreshTokenValue, int expiresInSeconds)
        {
            AccessTokenValue = accessTokenValue;
            RefreshTokenValue = refreshTokenValue;
            ExpiresInSeconds = expiresInSeconds;
        }

        public string AccessTokenValue { get; set; }
        public string RefreshTokenValue { get; set; }
        public int ExpiresInSeconds { get; set; }
    }
}