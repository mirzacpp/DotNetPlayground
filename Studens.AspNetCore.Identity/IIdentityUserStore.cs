using Microsoft.AspNetCore.Identity;

namespace Studens.AspNetCore.Identity;

/// <inheritdoc/>
public interface IIdentityUserStore<TUser> : IUserStore<TUser> where TUser : class
{
    Task<IList<TUser>> GetAsync(
        int? skip = null,
        int? take = null,
        string? normalizedUserName = null,
        string? normalizedEmail = null,
        CancellationToken cancellationToken = default);
}