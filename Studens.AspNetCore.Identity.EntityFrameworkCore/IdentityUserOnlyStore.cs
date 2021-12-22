using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Studens.AspNetCore.Identity.EntityFrameworkCore;

/// <inheritdoc />
public class IdentityUserOnlyStore<TUser> : IdentityUserOnlyStore<TUser, DbContext, string> where TUser : IdentityUser<string>, new()
{
    /// <inheritdoc />
    public IdentityUserOnlyStore(DbContext context, IdentityErrorDescriber describer = null) : base(context, describer) { }
}

/// <inheritdoc />
public class IdentityUserOnlyStore<TUser, TContext> : IdentityUserOnlyStore<TUser, TContext, string>
    where TUser : IdentityUser<string>
    where TContext : DbContext
{
    /// <inheritdoc />
    public IdentityUserOnlyStore(TContext context, IdentityErrorDescriber describer = null) : base(context, describer) { }
}

/// <inheritdoc />
public class IdentityUserOnlyStore<TUser, TContext, TKey> : IdentityUserOnlyStore<TUser, TContext, TKey, IdentityUserClaim<TKey>, IdentityUserLogin<TKey>, IdentityUserToken<TKey>>
    where TUser : IdentityUser<TKey>
    where TContext : DbContext
    where TKey : IEquatable<TKey>
{
    /// <inheritdoc />
    public IdentityUserOnlyStore(TContext context, IdentityErrorDescriber describer = null) : base(context, describer) { }
}

/// <inheritdoc />
public class IdentityUserOnlyStore<TUser, TContext, TKey, TUserClaim, TUserLogin, TUserToken> :
    UserOnlyStore<TUser, TContext, TKey, TUserClaim, TUserLogin, TUserToken>,
    IIdentityUserStore<TUser>
    where TUser : IdentityUser<TKey>
    where TContext : DbContext
    where TKey : IEquatable<TKey>
    where TUserClaim : IdentityUserClaim<TKey>, new()
    where TUserLogin : IdentityUserLogin<TKey>, new()
    where TUserToken : IdentityUserToken<TKey>, new()
{
    public IdentityUserOnlyStore(TContext context,
        IdentityErrorDescriber describer = null) :
        base(context, describer)
    {
    }

    public virtual async Task<IList<TUser>> GetAsync(
        int? skip = null,
        int? take = null,
        string? normalizedUserName = null,
        string? normalizedEmail = null,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        var query = Users;

        if (!string.IsNullOrEmpty(normalizedUserName))
        {
            query = query.Where(p => p.NormalizedUserName.Contains(normalizedUserName));
        }

        if (!string.IsNullOrEmpty(normalizedEmail))
        {
            query = query.Where(p => p.NormalizedEmail.Contains(normalizedEmail));
        }

        if (skip.HasValue && skip > 0)
        {
            query = query.Skip(skip.Value);
        }

        if (take.HasValue && take > 0)
        {
            query = query.Take(take.Value);
        }

        return await query.ToListAsync(cancellationToken);
    }
}