using Microsoft.AspNetCore.Identity;

namespace Studens.AspNetCore.Identity;

/// <inheritdoc />
public interface IIdentityRoleStore<TRole> : IRoleStore<TRole> where TRole : class
{
    Task<IList<TRole>> GetAsync(string? normalizedRoleName = null,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default);
}