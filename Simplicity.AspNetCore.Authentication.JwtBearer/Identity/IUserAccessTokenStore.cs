using Microsoft.AspNetCore.Identity;

namespace Simplicity.AspNetCore.Authentication.JwtBearer.Identity
{
    public interface IUserAccessTokenStore<TUser> : IUserStore<TUser> where TUser : class
    {
        Task<IdentityResult> AddAcessTokenAsync(TUser user,
            string accessTokenValue,
            DateTime accessTokenExpiresAtUtc,
            string refreshTokenValue,
            DateTime refreshTokenExpiresAtUtc,
            CancellationToken cancellationToken);

        Task<string?> GetAccessTokenAsync(TUser user, CancellationToken cancellationToken);

        //public virtual string AccessTokenValue { get; set; } = default!;

        ///// <summary>
        ///// Gets or sets the access token expiry date in UTC.
        ///// </summary>
        //public virtual DateTime AccessTokenExpiresAtUtc { get; set; }

        ///// <summary>
        ///// Gets or sets the creation date for this refresh token.
        ///// </summary>
        //public virtual DateTime CreatedAtUtc { get; set; }

        ///// <summary>
        ///// Gets or sets the refresh token value.
        ///// </summary>
        //[ProtectedPersonalData]
        //public virtual string RefreshTokenValue { get; set; } = default!;

        ///// <summary>
        ///// Gets or sets the expiration date for refresh token.
        ///// </summary>
        //public virtual DateTime RefreshTokenExpiresAtUtc { get; set; }
    }
}