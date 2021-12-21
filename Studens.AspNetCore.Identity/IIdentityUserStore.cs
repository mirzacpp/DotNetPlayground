using Microsoft.AspNetCore.Identity;

namespace Studens.AspNetCore.Identity;

/// <inheritdoc/>
public interface IIdentityUserStore<TUser> : IUserStore<TUser> where TUser : class
{
    Task<IEnumerable<TUser>> GetAllAsync(
        string? userName = null,
        string? email = null,
        CancellationToken cancellationToken = default);
}