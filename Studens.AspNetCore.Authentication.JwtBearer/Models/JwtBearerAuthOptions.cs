using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Studens.AspNetCore.Authentication.JwtBearer.Models
{
    /// <summary>
    /// Defines JWT options
    /// </summary>
    public class JwtBearerAuthOptions
    {
        public JwtBearerAuthOptions()
        {
            AccessTokenDuration = TimeSpan.FromMinutes(5);
            RefreshTokenDuration = TimeSpan.FromMinutes(24 * 60);
            Issuer = "Not null";
            Audience = "Not null";
            ///TODO: Validate secret is set on startup
            Secret = new string('S', 32);
        }

        /// <summary>
        /// Secret key used for token signing
        /// TODO: Validate this value on startup
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// Token issuer
        /// </summary>
        public string? Issuer { get; set; }

        /// <summary>
        /// Token audience
        /// </summary>
        public string? Audience { get; set; }

        /// <summary>
        /// Duration for a access token.
        /// Defaults to 5 minutes.
        /// </summary>
        public TimeSpan AccessTokenDuration { get; set; }

        /// <summary>
        /// Duration for a refresh token
        /// Defaults to 1440 minutes.
        /// </summary>
        public TimeSpan RefreshTokenDuration { get; set; }

        /// <summary>
        /// Determines if token refreshing should be supported
        /// </summary>
        public bool RefreshTokenEnabled { get; set; }
    }

    /// <summary>
    /// Internal security options used for token signatures.
    /// </summary>
    internal class JwtBearerAuthSecurityOptions
    {
        public SigningCredentials SigningCredentials { get; set; }
        public SymmetricSecurityKey SecurityKey { get; set; }
    }

    public class Testovka : IConfigureOptions<JwtBearerAuthOptions>
    {
        public void Configure(JwtBearerAuthOptions options)
        {
            throw new NotImplementedException();
        }
    }
}