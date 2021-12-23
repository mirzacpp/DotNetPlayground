namespace Studens.AspNetCore.Identity;

/// <inheritdoc />
public interface IIdentityRoleStore<TRole> : IRoleStore<TRole> where TRole : class
{
    Task<IList<TRole>> GetAsync(
        int? skip = null,
        int? take = null,
        string? normalizedRoleName = null,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default);
}