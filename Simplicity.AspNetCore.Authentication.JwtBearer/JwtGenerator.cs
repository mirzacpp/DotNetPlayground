using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Simplicity.AspNetCore.Authentication.JwtBearer.Models;
using Simplicity.Commons.Random;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Simplicity.AspNetCore.Authentication.JwtBearer
{
    public class JwtGenerator
    {
        private readonly JwtBearerAuthOptions _options;

        public JwtGenerator(IOptions<JwtBearerAuthOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;
        }

        /// <summary>
        /// Generates a JWT access and refresh tokens with defined claims.
        /// </summary>
        /// <param name="claims">Claims to include in token.</param>
        /// <returns>Generated tokens</returns>
        public (string accessToken, string refreshToken) GenerateTokens(IEnumerable<Claim> claims)
        {
            if (claims is null)
            {
                throw new ArgumentNullException(nameof(claims));
            }

            var key = Encoding.ASCII.GetBytes(_options.Secret);
            var utcNow = DateTime.UtcNow;
            var refreshTokenId = CryptoRandom.CreateUniqueId(format: CryptoRandom.OutputFormat.Base64);
            var extendedClaims = claims.ToList();

            // Append token related info
            extendedClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, refreshTokenId, ClaimValueTypes.String, _options.Issuer));
            extendedClaims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64, _options.Issuer));

            var token = new JwtSecurityToken(
             issuer: _options.Issuer,
             audience: _options.Audience,
             claims: extendedClaims,
             notBefore: utcNow,
             expires: utcNow.Add(_options.AccessTokenDuration),
             signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256));

            return (new JwtSecurityTokenHandler().WriteToken(token), refreshTokenId);            
        }
    }
}