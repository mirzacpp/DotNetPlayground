using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Studens.AspNetCore.Authentication.JwtBearer.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Studens.AspNetCore.Authentication.JwtBearer
{
    public class JwtGenerator : IJwtGenerator
    {
        private readonly JwtAuthenticationOptions _options;

        public JwtGenerator(IOptions<JwtAuthenticationOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;
        }

        /// <summary>
        /// Generates a JWT token with defined claims.
        /// </summary>
        /// <param name="claims">Claims to include in token.</param>
        /// <returns>Generated token resource</returns>
        public TokenResource GenerateToken(IEnumerable<Claim> claims)
        {
            if (claims is null)
            {
                throw new ArgumentNullException(nameof(claims));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_options.Secret);
            var utcNow = DateTime.UtcNow;
            var expiresAtUtc = utcNow.Add(_options.TokenGenerator.AccessTokenDuration);

            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(claims),
            //    Expires = DateTime.UtcNow.Add(_options.TokenGenerator.AccessTokenDuration),
            //    IssuedAt = DateTime.UtcNow,
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            //};

            var jwtToken = new JwtSecurityToken(
             issuer: _options.Bearer.ClaimsIssuer,
             audience: _options.Bearer.Audience,
             claims: claims,
             notBefore: utcNow,
             expires: expiresAtUtc,
             signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature));         

            return new TokenResource(
                tokenHandler.WriteToken(jwtToken),
                (int)_options.TokenGenerator.AccessTokenDuration.TotalSeconds);
        }

        public TokenResource GenerateRefreshToken()
        {
            return new TokenResource(
                Guid.NewGuid().ToString(),
                (int)_options.TokenGenerator.RefreshTokenDuration.TotalSeconds);
        }
    }
}