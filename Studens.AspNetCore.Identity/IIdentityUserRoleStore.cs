namespace Studens.AspNetCore.Identity;

/// <summary>
/// Provides an abstraction for a store which maps users to roles.
/// </summary>
/// <typeparam name="TUser">The type encapsulating a user.</typeparam>
public interface IIdentityUserRoleStore<TUser> :
    IIdentityUserStore<TUser>,
    IUserRoleStore<TUser> where TUser : class
{
    Task<bool> IsInRoleByIdAsync(TUser user, string roleId, CancellationToken cancellationToken);

    Task AddToRoleByIdAsync(TUser user, string roleId, CancellationToken cancellationToken);

    Task RemoveFromRoleByIdAsync(TUser user, string roleId, CancellationToken cancellationToken);
}