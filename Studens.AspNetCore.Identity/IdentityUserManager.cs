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

    /// <summary>
    /// Gets or sets the persistence store the manager operates over.
    /// </summary>
    /// <value>The persistence store the manager operates over.</value>
    protected IIdentityUserStore<TUser> IdentityUserStore { get; }

    /// <summary>
    /// The cancellation token associated with the current HttpContext.RequestAborted or CancellationToken.None if unavailable.
    /// </summary>
    protected override CancellationToken CancellationToken => _cancellationToken;

    #region Ctor

    public IdentityUserManager(IIdentityUserStore<TUser> store,
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
        IdentityUserStore = store;

        _cancellationToken = services
            ?.GetService<IHttpContextAccessor>()?.HttpContext?.RequestAborted ?? CancellationToken.None;
    }

    #endregion Ctor

    public virtual Task<IEnumerable<TUser>> GetAllAsync(string? userName = null,
        string? email = null)
    {
        ThrowIfDisposed();

        return IdentityUserStore.GetAllAsync(userName, email, CancellationToken);
    }
}