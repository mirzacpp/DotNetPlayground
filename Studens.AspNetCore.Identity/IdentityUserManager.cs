namespace Studens.AspNetCore.Identity;

/// <inheritdoc/>
/// TODO: Maybe resolve <see cref="CancellationToken"/> from external service so we do not depend on <see cref="IHttpContextAccessor"/>
public partial class IdentityUserManager<TUser> : UserManager<TUser> where TUser : class
{
    #region Fields

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

    #endregion Fields

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
        await userRoleStore.AddToRoleByIdAsync(user, roleId, CancellationToken);
        return await UpdateUserAsync(user);
    }

    public virtual async Task<IdentityResult> AddToRolesByIdAsync(TUser user, IEnumerable<string> roleIds)
    {
        ThrowIfDisposed();
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        if (roleIds is null)
        {
            throw new ArgumentNullException(nameof(roleIds));
        }

        var userRoleStore = GetUserRoleStore();

        foreach (var roleId in roleIds.Distinct())
        {
            if (await userRoleStore.IsInRoleByIdAsync(user, roleId, CancellationToken))
            {
                return UserAlreadyInRoleError(roleId);
            }
            await userRoleStore.AddToRoleByIdAsync(user, roleId, CancellationToken);
        }
        return await UpdateUserAsync(user);
    }

    public virtual async Task<IdentityResult> RemoveFromRoleByIdAsync(TUser user, string roleId)
    {
        ThrowIfDisposed();
        var userRoleStore = GetUserRoleStore();
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (!await userRoleStore.IsInRoleByIdAsync(user, roleId, CancellationToken))
        {
            return UserNotInRoleError(roleId);
        }
        await userRoleStore.RemoveFromRoleByIdAsync(user, roleId, CancellationToken);
        return await UpdateUserAsync(user);
    }

    public virtual async Task<IdentityResult> RemoveFromRolesByIdAsync(TUser user, IEnumerable<string> roleIds)
    {
        ThrowIfDisposed();
        var userRoleStore = GetUserRoleStore();
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        if (roleIds == null)
        {
            throw new ArgumentNullException(nameof(roleIds));
        }

        foreach (var roleId in roleIds.Distinct())
        {
            if (!await userRoleStore.IsInRoleByIdAsync(user, roleId, CancellationToken))
            {
                return UserNotInRoleError(roleId);
            }
            await userRoleStore.RemoveFromRoleByIdAsync(user, roleId, CancellationToken);
        }
        return await UpdateUserAsync(user);
    }

    #endregion Methods

    #region Utils

    private IIdentityUserRoleStore<TUser> GetUserRoleStore()
    {
        if (Store is not IIdentityUserRoleStore<TUser> cast)
        {
            throw new NotSupportedException("Store not IIdentityUserRoleStore.");
        }
        return cast;
    }

    private IdentityResult UserAlreadyInRoleError(string roleId)
    {
        Logger.LogWarning(LoggerEventIds.UserAlreadyInRole, "User is already in role with id {roleId}.", roleId);
        return IdentityResult.Failed(ErrorDescriber.UserAlreadyInRole(roleId));
    }

    private IdentityResult UserNotInRoleError(string roleId)
    {
        Logger.LogWarning(LoggerEventIds.UserNotInRole, "User not in role with id {roleId}.", roleId);
        return IdentityResult.Failed(ErrorDescriber.UserNotInRole(roleId));
    }

    #endregion Utils
}