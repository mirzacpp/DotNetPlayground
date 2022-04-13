using Microsoft.AspNetCore.Identity;

namespace Studens.AspNetCore.Authentication.JwtBearer.Models
{
    /// <summary>
    /// Represents a user access token that a user possesses.
    /// </summary>
    /// <typeparam name="TKey">The type used for the primary key for this user that possesses this claim.</typeparam>
    public class IdentityUserAccessToken<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets or sets the identifier for this refresh token.
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Gets or sets the primary key of the user associated with refresh token.
        /// </summary>
        public virtual TKey UserId { get; set; }

        /// <summary>
        /// Gets or sets the access token value.
        /// </summary>
        [ProtectedPersonalData]
        public virtual string AccessTokenValue { get; set; } = default!;

        /// <summary>
        /// Gets or sets the access token expiry date in UTC.
        /// </summary>
        public virtual DateTime AccessTokenExpiresAtUtc { get; set; }

        /// <summary>
        /// Gets or sets the creation date for this refresh token.
        /// </summary>
        public virtual DateTime CreatedAtUtc { get; set; }

        /// <summary>
        /// Gets or sets the refresh token value.
        /// </summary>
        [ProtectedPersonalData]
        public virtual string RefreshTokenValue { get; set; } = default!;

        /// <summary>
        /// Gets or sets the expiration date for refresh token.
        /// </summary>
        public virtual DateTime RefreshTokenExpiresAtUtc { get; set; }     

        /// <summary>
        /// Gets or sets the identifier of the token.
        /// </summary>
        public virtual string? TokenId { get; set; }

        /// <summary>
        /// Returns the access token value for this token.
        /// </summary>
        public override string ToString() => AccessTokenValue;
    }
}