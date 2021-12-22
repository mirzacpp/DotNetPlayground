using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Studens.AspNetCore.Identity;

/// <inheritdoc/>
/// TODO: Maybe resolve <see cref="CancellationToken"/> from external service so we do not depend on <see cref="IHttpContextAccessor"/>
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

    #region Methods

    public virtual Task<IList<TUser>> GetAsync(int? skip = null,
        int? take = null,
        string? userName = null,
        string? email = null)
    {
        ThrowIfDisposed();

        return IdentityUserStore.GetAsync(skip, take, NormalizeName(userName), NormalizeEmail(email), CancellationToken);
    }

    public virtual async Task<IdentityResult> AddToRoleByIdAsync(TUser user, string roleId)
    {
        ThrowIfDisposed();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        var userRoleStore = GetUserRoleStore();
        if (await userRoleStore.IsInRoleByIdAsync(user, roleId, CancellationToken))
        {
            return UserAlreadyInRoleError(roleId);
        }
        await userRoleStore.AddToRoleAsync(user, roleId, CancellationToken);
        return await UpdateUserAsync(user);
    }

    #endregion Methods

    private IIdentityUserRoleStore<TUser> GetUserRoleStore()
    {
        var cast = Store as IIdentityUserRoleStore<TUser>;
        if (cast == null)
        {
            throw new NotSupportedException("Store Not IUserRoleStore");
        }
        return cast;
    }

    private IdentityResult UserAlreadyInRoleError(string roleId)
    {
        //Logger.LogWarning(LoggerEventIds.UserAlreadyInRole, "User is already in role with id {roleId}.", role);
        return IdentityResult.Failed(ErrorDescriber.UserAlreadyInRole(roleId));
    }    
}