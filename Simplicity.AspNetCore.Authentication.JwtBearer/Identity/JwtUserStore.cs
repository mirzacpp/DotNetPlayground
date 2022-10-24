using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Simplicity.AspNetCore.Authentication.JwtBearer.Models;

namespace Simplicity.AspNetCore.Authentication.JwtBearer.Identity
{
    /// <summary>
    /// Creates a new instance of a persistence store for the specified user type.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user.</typeparam>
    public class JwtUserStore<TUser, TUserAccessToken> : UserStore<TUser, IdentityRole, DbContext, string>
        where TUser : IdentityUser<string>, new()
        where TUserAccessToken : IdentityUserAccessToken<string>, new()
    {
        /// <summary>
        /// Constructs a new instance of <see cref="UserStore{TUser}"/>.
        /// </summary>
        /// <param name="context">The <see cref="DbContext"/>.</param>
        /// <param name="describer">The <see cref="IdentityErrorDescriber"/>.</param>
        public JwtUserStore(DbContext context, IdentityErrorDescriber? describer = null) : base(context, describer) { }
    }

    /// <summary>
    /// Represents a new instance of a persistence store for the specified user and role types.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user.</typeparam>
    /// <typeparam name="TRole">The type representing a role.</typeparam>
    /// <typeparam name="TContext">The type of the data context class used to access the store.</typeparam>
    public class JwtUserStore<TUser, TUserAccessToken, TRole, TContext> : UserStore<TUser, TRole, TContext, string>
        where TUser : IdentityUser<string>
        where TUserAccessToken : IdentityUserAccessToken<string>
        where TRole : IdentityRole<string>
        where TContext : DbContext
    {
        /// <summary>
        /// Constructs a new instance of <see cref="UserStore{TUser, TRole, TContext}"/>.
        /// </summary>
        /// <param name="context">The <see cref="DbContext"/>.</param>
        /// <param name="describer">The <see cref="IdentityErrorDescriber"/>.</param>
        public JwtUserStore(TContext context, IdentityErrorDescriber? describer = null) : base(context, describer) { }
    }

    /// <summary>
    /// Represents a new instance of a persistence store for the specified user and role types.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user.</typeparam>
    /// <typeparam name="TRole">The type representing a role.</typeparam>
    /// <typeparam name="TContext">The type of the data context class used to access the store.</typeparam>
    /// <typeparam name="TKey">The type of the primary key for a role.</typeparam>
    public class JwtUserStore<TUser, TUserAccessToken, TRole, TContext, TKey> : UserStore<TUser, TRole, TContext, TKey, IdentityUserClaim<TKey>, IdentityUserRole<TKey>, IdentityUserLogin<TKey>, IdentityUserToken<TKey>, IdentityRoleClaim<TKey>>
        where TUser : IdentityUser<TKey>
        where TUserAccessToken : IdentityUserAccessToken<TKey>
        where TRole : IdentityRole<TKey>
        where TContext : DbContext
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Constructs a new instance of <see cref="UserStore{TUser, TRole, TContext, TKey}"/>.
        /// </summary>
        /// <param name="context">The <see cref="DbContext"/>.</param>
        /// <param name="describer">The <see cref="IdentityErrorDescriber"/>.</param>
        public JwtUserStore(TContext context, IdentityErrorDescriber? describer = null) : base(context, describer) { }
    }

    /// <summary>
    /// Represents a new instance of a persistence store for the specified user and role types.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user.</typeparam>
    /// <typeparam name="TRole">The type representing a role.</typeparam>
    /// <typeparam name="TContext">The type of the data context class used to access the store.</typeparam>
    /// <typeparam name="TKey">The type of the primary key for a role.</typeparam>
    /// <typeparam name="TUserClaim">The type representing a claim.</typeparam>
    /// <typeparam name="TUserRole">The type representing a user role.</typeparam>
    /// <typeparam name="TUserLogin">The type representing a user external login.</typeparam>
    /// <typeparam name="TUserToken">The type representing a user token.</typeparam>
    /// <typeparam name="TRoleClaim">The type representing a role claim.</typeparam>
    public class JwtUserStore<TUser, TUserAccessToken, TRole, TContext, TKey, TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim> :
        UserStore<TUser, TRole, TContext, TKey, TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim>,
        IProtectedUserStore<TUser>,
        IUserAccessTokenStore<TUser>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TContext : DbContext
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>, new()
        where TUserRole : IdentityUserRole<TKey>, new()
        where TUserLogin : IdentityUserLogin<TKey>, new()
        where TUserToken : IdentityUserToken<TKey>, new()
        where TRoleClaim : IdentityRoleClaim<TKey>, new()
        where TUserAccessToken : IdentityUserAccessToken<TKey>, new()
    {
        public JwtUserStore(TContext context, IdentityErrorDescriber? describer = null) : base(context, describer)
        {
        }

        private DbSet<TUserAccessToken> UserAccessTokens => Context.Set<TUserAccessToken>();

        public async Task<IdentityResult> AddAcessTokenAsync(TUser user,
            string accessTokenValue,
            DateTime accessTokenExpiresAtUtc,
            string refreshTokenValue,
            DateTime refreshTokenExpiresAtUtc,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var accessToken = new TUserAccessToken
            {
                AccessTokenValue = accessTokenValue,
                AccessTokenExpiresAtUtc = accessTokenExpiresAtUtc,
                RefreshTokenValue = refreshTokenValue,
                RefreshTokenExpiresAtUtc = refreshTokenExpiresAtUtc,
                CreatedAtUtc = DateTime.UtcNow,
                UserId = user.Id
            };

            UserAccessTokens.Add(accessToken);  
            await SaveChanges(cancellationToken);

            return IdentityResult.Success;
        }

        public async Task<string?> GetAccessTokenAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return (await UserAccessTokens.FirstOrDefaultAsync(uat => uat.UserId.Equals(user.Id), cancellationToken))?.AccessTokenValue;
        }
    }
}