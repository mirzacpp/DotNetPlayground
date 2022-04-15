using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Studens.AspNetCore.Authentication.JwtBearer.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Studens.AspNetCore.Authentication.JwtBearer.Identity
{
    /// <summary>
    /// Provides the APIs for user sign in.
    /// </summary>
    /// <typeparam name="TUser">The type encapsulating a user.</typeparam>
    public class JwtSignInManager<TUser> : SignInManager<TUser> where TUser : class
    {
        public JwtSignInManager(
            JwtUserManager<TUser> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<TUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<TUser>> logger,
            IAuthenticationSchemeProvider schemes,
            IUserConfirmation<TUser> confirmation,
            JwtGenerator jwtGenerator,
            IOptions<JwtBearerAuthOptions> jwtBearerOptionsAccessor) : base(
                userManager,
                contextAccessor,
                claimsFactory,
                optionsAccessor,
                logger,
                schemes,
                confirmation)
        {
            JwtUserManager = userManager;
            JwtGenerator = jwtGenerator;
            JwtBearerOptions = jwtBearerOptionsAccessor.Value;
        }

        /// <summary>
        /// The <see cref="JwtUserManager{TUser}"/> used.
        /// </summary>
        public JwtUserManager<TUser> JwtUserManager { get; set; }

        /// <summary>
        /// The <see cref="JwtGenerator"/> used.
        /// </summary>
        public JwtGenerator JwtGenerator { get; set; }

        /// <summary>
        /// The <see cref="Models.JwtBearerAuthOptions"/> used.
        /// </summary>
        public JwtBearerAuthOptions JwtBearerOptions { get; }

        /// <summary>
        /// Creates a JWT access token for specified <paramref name="user"/>.
        /// </summary>
        /// <returns>
        /// ... Follow identity summary pattern
        /// </returns>
        public virtual async Task<AuthenticationResult> CreateAccessTokenAsync(TUser user)
        {
            Guard.Against.Null(user, nameof(user));

            var userId = await JwtUserManager.GetUserIdAsync(user);

            var userClaims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId)
            };

            var (accessToken, refreshToken) = JwtGenerator.GenerateTokens(userClaims);

            await JwtUserManager.AddAcessTokenAsync(user,
                accessToken,
                DateTime.UtcNow.Add(JwtBearerOptions.AccessTokenDuration),
                refreshToken,
                DateTime.UtcNow.Add(JwtBearerOptions.RefreshTokenDuration));

            return new AuthenticationResult
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAtInSeconds = (int)JwtBearerOptions.RefreshTokenDuration.TotalSeconds,
                UserId = userId,
            };
        }       
    }
}