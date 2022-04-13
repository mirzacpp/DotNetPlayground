using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Studens.AspNetCore.Authentication.JwtBearer.Models
{
    /// <summary>
    /// Defines JWT options
    /// </summary>
    public class JwtAuthenticationOptions
    {
        public JwtAuthenticationOptions()
        {
            TokenGenerator = new JwtGeneratorOptions();
            Bearer = new JwtBearerOptions();
            Secret = string.Empty;
        }

        public JwtGeneratorOptions TokenGenerator { get; set; }
        public JwtBearerOptions Bearer { get; set; }
        public string Secret { get; set; }
    }

    public class JwtGeneratorOptions
    {
        public JwtGeneratorOptions()
        {
            AccessTokenDuration = TimeSpan.FromMinutes(5);
            RefreshTokenDuration = TimeSpan.FromMinutes(24 * 60);
        }

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
    }
}