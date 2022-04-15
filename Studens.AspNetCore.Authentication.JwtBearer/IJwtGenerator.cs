using Studens.AspNetCore.Authentication.JwtBearer.Models;
using System.Security.Claims;

namespace Studens.AspNetCore.Authentication.JwtBearer
{
    /// <summary>
    /// Defines contract for JWT token generation.
    /// </summary>
    public interface IJwtGenerator
    {
        /// <summary>
        /// Generates a JWT token with defined claims.
        /// </summary>
        /// <param name="claims">Claims to include in token.</param>
        /// <returns>Generated token resource</returns>
        TokenResource GenerateTokens(IEnumerable<Claim> claims);        
    }
}