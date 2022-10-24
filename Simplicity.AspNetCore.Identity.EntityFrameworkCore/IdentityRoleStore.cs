using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Simplicity.AspNetCore.Identity.EntityFrameworkCore;

/// <summary>
/// <inheritdoc />
/// </summary>
public class IdentityRoleStore<TRole> : IdentityRoleStore<TRole, DbContext, string>
    where TRole : IdentityRole<string>
{
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public IdentityRoleStore(DbContext context,
        IdentityErrorDescriber describer = null) :
        base(context, describer)
    { }
}

/// <summary>
/// <inheritdoc />
/// </summary>
public class IdentityRoleStore<TRole, TContext> : IdentityRoleStore<TRole, TContext, string>
    where TRole : IdentityRole<string>
    where TContext : DbContext
{
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public IdentityRoleStore(TContext context,
        IdentityErrorDescriber describer = null) :
        base(context, describer)
    { }
}

/// <summary>
/// <inheritdoc />
/// </summary>
public class IdentityRoleStore<TRole, TContext, TKey> : IdentityRoleStore<TRole, TContext, TKey, IdentityUserRole<TKey>, IdentityRoleClaim<TKey>>,
    IQueryableRoleStore<TRole>,
    IRoleClaimStore<TRole>
    where TRole : IdentityRole<TKey>
    where TKey : IEquatable<TKey>
    where TContext : DbContext
{
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public IdentityRoleStore(TContext context,
        IdentityErrorDescriber describer = null) :
        base(context, describer)
    { }
}

/// <summary>
/// <inheritdoc />
/// </summary>
public class IdentityRoleStore<TRole, TContext, TKey, TUserRole, TRoleClaim> :
    RoleStore<TRole, TContext, TKey, TUserRole, TRoleClaim>,
    IIdentityRoleStore<TRole>
    where TRole : IdentityRole<TKey>
    where TKey : IEquatable<TKey>
    where TContext : DbContext
    where TUserRole : IdentityUserRole<TKey>, new()
    where TRoleClaim : IdentityRoleClaim<TKey>, new()

{
    public IdentityRoleStore(TContext context,
        IdentityErrorDescriber describer = null) :
        base(context, describer)
    {
    }

    public virtual async Task<IList<TRole>> GetAsync(
        int? skip = null,
        int? take = null,
        string? normalizedRoleName = null,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        var query = asNoTracking ? Roles.AsNoTracking() : Roles;

        if (!string.IsNullOrEmpty(normalizedRoleName))
        {
            query = query.Where(p => p.NormalizedName.Contains(normalizedRoleName));
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