using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Studens.AspNetCore.Identity.EntityFrameworkCore
{
    /// <inheritdoc />
    public class IdentityUserStore : IdentityUserStore<IdentityUser<string>>
    {
        /// <inheritdoc/>
        public IdentityUserStore(DbContext context, IdentityErrorDescriber describer = null) : base(context, describer) { }
    }

    /// <summary>
    /// Creates a new instance of a persistence store for the specified user type.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user.</typeparam>
    public class IdentityUserStore<TUser> : IdentityUserStore<TUser, IdentityRole, DbContext, string>
        where TUser : IdentityUser<string>, new()
    {
        /// <summary>
        /// Constructs a new instance of <see cref="UserStore{TUser}"/>.
        /// </summary>
        /// <param name="context">The <see cref="DbContext"/>.</param>
        /// <param name="describer">The <see cref="IdentityErrorDescriber"/>.</param>
        public IdentityUserStore(DbContext context, IdentityErrorDescriber describer = null) : base(context, describer) { }
    }

    /// <summary>
    /// Represents a new instance of a persistence store for the specified user and role types.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user.</typeparam>
    /// <typeparam name="TRole">The type representing a role.</typeparam>
    /// <typeparam name="TContext">The type of the data context class used to access the store.</typeparam>
    public class IdentityUserStore<TUser, TRole, TContext> : IdentityUserStore<TUser, TRole, TContext, string>
        where TUser : IdentityUser<string>
        where TRole : IdentityRole<string>
        where TContext : DbContext
    {
        /// <summary>
        /// Constructs a new instance of <see cref="UserStore{TUser, TRole, TContext}"/>.
        /// </summary>
        /// <param name="context">The <see cref="DbContext"/>.</param>
        /// <param name="describer">The <see cref="IdentityErrorDescriber"/>.</param>
        public IdentityUserStore(TContext context, IdentityErrorDescriber describer = null) : base(context, describer) { }
    }

    /// <summary>
    /// Represents a new instance of a persistence store for the specified user and role types.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user.</typeparam>
    /// <typeparam name="TRole">The type representing a role.</typeparam>
    /// <typeparam name="TContext">The type of the data context class used to access the store.</typeparam>
    /// <typeparam name="TKey">The type of the primary key for a role.</typeparam>
    public class IdentityUserStore<TUser, TRole, TContext, TKey> : IdentityUserStore<TUser, TRole, TContext, TKey, IdentityUserClaim<TKey>, IdentityUserRole<TKey>, IdentityUserLogin<TKey>, IdentityUserToken<TKey>, IdentityRoleClaim<TKey>>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TContext : DbContext
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Constructs a new instance of <see cref="UserStore{TUser, TRole, TContext, TKey}"/>.
        /// </summary>
        /// <param name="context">The <see cref="DbContext"/>.</param>
        /// <param name="describer">The <see cref="IdentityErrorDescriber"/>.</param>
        public IdentityUserStore(TContext context, IdentityErrorDescriber describer = null) : base(context, describer) { }
    }

    /// <inheritdoc/>
    public class IdentityUserStore<TUser, TRole, TContext, TKey, TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim> :
        UserStore<TUser, TRole, TContext, TKey, TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim>,
        IIdentityUserStore<TUser>,
        IIdentityUserRoleStore<TUser>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TContext : DbContext
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>, new()
        where TUserRole : IdentityUserRole<TKey>, new()
        where TUserLogin : IdentityUserLogin<TKey>, new()
        where TUserToken : IdentityUserToken<TKey>, new()
        where TRoleClaim : IdentityRoleClaim<TKey>, new()
    {
        public IdentityUserStore(TContext context,
            IdentityErrorDescriber describer = null)
            : base(context, describer)
        {
        }

        private DbSet<TUser> UsersSet => Context.Set<TUser>();
        private DbSet<TRole> Roles => Context.Set<TRole>();
        private DbSet<TUserClaim> UserClaims => Context.Set<TUserClaim>();
        private DbSet<TUserRole> UserRoles => Context.Set<TUserRole>();
        private DbSet<TUserLogin> UserLogins => Context.Set<TUserLogin>();
        private DbSet<TUserToken> UserTokens => Context.Set<TUserToken>();

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

        public virtual async Task<bool> IsInRoleByIdAsync(TUser user, string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrWhiteSpace(roleId))
            {
                throw new ArgumentException("Value cannot be null or empty", nameof(roleId));
            }

            var role = await FindRoleByIdAsync(roleId, cancellationToken);

            if (role != null)
            {
                var userRole = await FindUserRoleAsync(user.Id, role.Id, cancellationToken);
                return userRole != null;
            }

            return false;
        }

        /// <summary>
        /// Adds the given <paramref name="roleId"/> to the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user to add the role to.</param>
        /// <param name="normalizedRoleName">The role identifier to add.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public virtual async Task AddToRoleByIdAsync(TUser user, string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrWhiteSpace(roleId))
            {
                throw new ArgumentException("Value cannot be null or empty", nameof(roleId));
            }

            var roleEntity = await FindRoleByIdAsync(roleId, cancellationToken);
            if (roleEntity == null)
            {
                throw new InvalidOperationException($"Role with identifier {roleId} could not be found.");
            }

            UserRoles.Add(CreateUserRole(user, roleEntity));
        }

        public virtual async Task RemoveFromRoleByIdAsync(TUser user, string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (string.IsNullOrWhiteSpace(roleId))
            {
                throw new ArgumentException("Value cannot be null or empty", nameof(roleId));
            }
            var roleEntity = await FindRoleByIdAsync(roleId, cancellationToken);
            if (roleEntity != null)
            {
                var userRole = await FindUserRoleAsync(user.Id, roleEntity.Id, cancellationToken);
                if (userRole != null)
                {
                    UserRoles.Remove(userRole);
                }
            }
        }

        /// <summary>
        /// Return a role with the role identifier if it exists.
        /// </summary>
        /// <param name="normalizedRoleName">The role identifier.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The role if it exists.</returns>
        protected virtual Task<TRole?> FindRoleByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            var id = ConvertIdFromString(roleId);

            return Roles.SingleOrDefaultAsync(r => r.Id.Equals(id), cancellationToken);
        }
    }
}