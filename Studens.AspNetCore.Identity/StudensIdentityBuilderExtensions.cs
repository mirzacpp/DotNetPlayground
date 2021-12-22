using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Studens.AspNetCore.Identity;

/// <summary>
/// Helper functions for configuring identity services.
/// </summary>
public static class StudensIdentityBuilderExtensions
{
    /// <summary>
    /// Adds a <see cref="IdentityUserManager{TUser}"/> for the <see cref="IdentityBuilder.UserType"/>.
    /// </summary>
    /// <returns>The current <see cref="IdentityBuilder"/> instance.</returns>
    public static IdentityBuilder AddStudensUserManager(this IdentityBuilder builder)
    {
        var userManagerType = typeof(UserManager<>).MakeGenericType(builder.UserType);
        var customType = typeof(IdentityUserManager<>).MakeGenericType(builder.UserType);

        builder.Services.AddScoped(customType, services => services.GetRequiredService(userManagerType));
        builder.Services.AddScoped(userManagerType, customType);

        return builder;
    }

    /// <summary>
    /// Adds a <see cref="IdentityRoleManager{TRole}"/> for the <see cref="IdentityBuilder.RoleType"/>.
    /// </summary>
    /// <returns>The current <see cref="IdentityBuilder"/> instance.</returns>
    public static IdentityBuilder AddStudensRoleManager(this IdentityBuilder builder)
    {
        var roleManagerType = typeof(RoleManager<>).MakeGenericType(builder.RoleType);
        var customType = typeof(IdentityRoleManager<>).MakeGenericType(builder.RoleType);

        builder.Services.AddScoped(customType, services => services.GetRequiredService(roleManagerType));
        builder.Services.AddScoped(roleManagerType, customType);

        return builder;
    }
}