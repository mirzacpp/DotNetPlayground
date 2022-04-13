using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Studens.AspNetCore.Authentication.JwtBearer.Identity
{
    /// <summary>
    /// Provides the APIs for managing user in a persistence store.
    /// </summary>
    /// <typeparam name="TUser">The type encapsulating a user.</typeparam>
    public class JwtUserManager<TUser, TUserAccessToken> : UserManager<TUser>, IDisposable
        where TUser : class
        where TUserAccessToken : class
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

        private IUserAccessTokenStore<TUser, TUserAccessToken> GetAccessTokenStore()
        {
            var cast = Store as IUserAccessTokenStore<TUser, TUserAccessToken>;
            if (cast == null)
            {
                throw new NotSupportedException("Resource error here mate.");
            }
            return cast;
        }

        public async Task<IdentityResult> AddAcessTokenAsync(TUser user, TUserAccessToken accessToken)
        {
            ThrowIfDisposed();
            var store = GetAccessTokenStore();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (accessToken == null)
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            await store.AddAcessTokenAsync(user, accessToken, CancellationToken);
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