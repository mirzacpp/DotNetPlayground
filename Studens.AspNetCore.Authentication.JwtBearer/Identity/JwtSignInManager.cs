using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Studens.AspNetCore.Authentication.JwtBearer.Identity
{
    /// <summary>
    /// Provides the APIs for user sign in.
    /// </summary>
    /// <typeparam name="TUser">The type encapsulating a user.</typeparam>
    public class JwtSignInManager<TUser, TUserAccessToken> :
        SignInManager<TUser> where TUser : class where TUserAccessToken : class
    {
        public JwtSignInManager(
            UserManager<TUser> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<TUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<TUser>> logger,
            IAuthenticationSchemeProvider schemes,
            IUserConfirmation<TUser> confirmation) : base(
                userManager,
                contextAccessor,
                claimsFactory,
                optionsAccessor,
                logger,
                schemes,
                confirmation)
        {
        }

        /// <summary>
        /// Creates a JWT access token for specified <paramref name="user"/>.
        /// </summary>
        /// <returns>
        /// ... Follow identity summary pattern
        /// </returns>
        public virtual async Task<TUserAccessToken> CreateAccessTokenAsync(TUser user)
        {
            return null;
        }
    }
}