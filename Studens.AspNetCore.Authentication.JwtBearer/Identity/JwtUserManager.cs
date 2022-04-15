using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Studens.AspNetCore.Authentication.JwtBearer.Identity
{
    /// <summary>
    /// Provides the APIs for managing user in a persistence store.
    /// </summary>
    /// <typeparam name="TUser">The type encapsulating a user.</typeparam>
    public class JwtUserManager<TUser> : UserManager<TUser>, IDisposable
        where TUser : class        
    {
        public JwtUserManager(
            IUserStore<TUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<TUser> passwordHasher,
            IEnumerable<IUserValidator<TUser>> userValidators,
            IEnumerable<IPasswordValidator<TUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<TUser>> logger) : base(
                store,
                optionsAccessor,
                passwordHasher,
                userValidators,
                passwordValidators,
                keyNormalizer,
                errors,
                services,
                logger)
        {
        }

        private IUserAccessTokenStore<TUser> GetAccessTokenStore()
        {
            var cast = Store as IUserAccessTokenStore<TUser>;
            if (cast == null)
            {
                throw new NotSupportedException("Resource error here mate.");
            }
            return cast;
        }

        public async Task<IdentityResult> AddAcessTokenAsync(TUser user, string
            accessTokenValue,
            DateTime accessTokenExpiresAtUtc,
            string refreshTokenValue,
            DateTime refreshTokenExpiresAtUtc)
        {
            ThrowIfDisposed();
            var store = GetAccessTokenStore();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            await store.AddAcessTokenAsync(user, 
                accessTokenValue, 
                accessTokenExpiresAtUtc, 
                refreshTokenValue, 
                refreshTokenExpiresAtUtc, 
                CancellationToken);
            return await UpdateUserAsync(user);
        }

        public async Task<string> GetAccessTokenAsync(TUser user)
        {
            ThrowIfDisposed();
            var store = GetAccessTokenStore();

            var token = await store.GetAccessTokenAsync(user, CancellationToken);

            return "Fake token";
        }
    }
}