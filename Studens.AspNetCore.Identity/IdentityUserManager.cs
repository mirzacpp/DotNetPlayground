using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Studens.AspNetCore.Identity;

/// <summary>
/// Extends existing <see cref="UserManager{TUser}"/> with additional methods.
/// </summary>
/// <typeparam name="TUser">The type encapsulating a user.</typeparam>
public partial class IdentityUserManager<TUser> : UserManager<TUser> where TUser : class
{
    private readonly CancellationToken _cancellationToken;

    #region Ctor

    public IdentityUserManager(IUserStore<TUser> store,
     IOptions<IdentityOptions> optionsAccessor,
     IPasswordHasher<TUser> passwordHasher,
     IEnumerable<IUserValidator<TUser>> userValidators,
     IEnumerable<IPasswordValidator<TUser>> passwordValidators,
     ILookupNormalizer keyNormalizer,
     IdentityErrorDescriber errors,
     IServiceProvider services,
     ILogger<UserManager<TUser>> logger) : base(store,
         optionsAccessor,
         passwordHasher,
         userValidators,
         passwordValidators,
         keyNormalizer,
         errors,
         services,
         logger)
    {
        _cancellationToken = services
            ?.GetService<IHttpContextAccessor>()?.HttpContext?.RequestAborted ?? CancellationToken.None;
    }

    #endregion Ctor

    /// <summary>
    /// The cancellation token associated with the current HttpContext.RequestAborted or CancellationToken.None if unavailable.
    /// </summary>
    protected override CancellationToken CancellationToken => _cancellationToken;
}