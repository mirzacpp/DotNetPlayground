using Microsoft.AspNetCore.Identity;

namespace Studens.AspNetCore.Identity;

/// <summary>
/// Extends default <see cref="IUserStore{TUser}"/>
/// </summary>
/// <typeparam name="TUser">Identity user type</typeparam>
public interface IIdentityUserStore<TUser> : IUserStore<TUser> where TUser : class
{
    Task<IEnumerable<TUser>> GetAllAsync(
        string? userName = null,
        string? email = null,
        CancellationToken cancellationToken = default);
}