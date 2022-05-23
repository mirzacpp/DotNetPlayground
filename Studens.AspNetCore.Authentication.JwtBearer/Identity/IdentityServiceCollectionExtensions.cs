using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Studens.AspNetCore.Authentication.JwtBearer.Identity;
using Studens.AspNetCore.Authentication.JwtBearer.Models;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Contains extension methods to <see cref="IServiceCollection"/> for configuring identity services.
    /// </summary>
    public static class IdentityServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the default JWT identity system configuration for the specified User and Role types.
        /// </summary>
        /// <typeparam name="TUser">The type representing a User in the system.</typeparam>
        /// <typeparam name="TRole">The type representing a Role in the system.</typeparam>
        /// <param name="services">The services available in the application.</param>
        /// <returns>An <see cref="IdentityBuilder"/> for creating and configuring the identity system.</returns>
        public static IdentityBuilder AddJwtBearerIdentity<TUser, TRole, TUserAccessToken>(
            this IServiceCollection services,
            Action<JwtBearerAuthOptions> bearerSetupAction)
            where TUser : class
            where TRole : class
            where TUserAccessToken : class
            => services.AddJwtBearerIdentity<TUser, TRole, TUserAccessToken>(bearerSetupAction, setupAction: null);

        /// <summary>
        /// Adds and configures the JWT identity system for the specified User and Role types.
        /// </summary>
        /// <typeparam name="TUser">The type representing a User in the system.</typeparam>
        /// <typeparam name="TRole">The type representing a Role in the system.</typeparam>
        /// <param name="services">The services available in the application.</param>
        /// <param name="setupAction">An action to configure the <see cref="IdentityOptions"/>.</param>
        /// <returns>An <see cref="IdentityBuilder"/> for creating and configuring the identity system.</returns>
        public static IdentityBuilder AddJwtBearerIdentity<TUser, TRole, TUserAccessToken>(
            this IServiceCollection services,
            Action<JwtBearerAuthOptions> bearerSetupAction,
            Action<IdentityOptions>? setupAction)
            where TUser : class
            where TRole : class
            where TUserAccessToken : class
        {
            // Register jwt auth handlers
            services.AddJwtBearerAuthentication(bearerSetupAction); 

            // Hosting doesn't add IHttpContextAccessor by default
            services.AddHttpContextAccessor();
            // Identity services
            services.TryAddScoped<IUserValidator<TUser>, UserValidator<TUser>>();
            services.TryAddScoped<IPasswordValidator<TUser>, PasswordValidator<TUser>>();
            services.TryAddScoped<IPasswordHasher<TUser>, PasswordHasher<TUser>>();
            services.TryAddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
            services.TryAddScoped<IRoleValidator<TRole>, RoleValidator<TRole>>();
            // No interface for the error describer so we can add errors without rev'ing the interface
            services.TryAddScoped<IdentityErrorDescriber>();
            services.TryAddScoped<ISecurityStampValidator, SecurityStampValidator<TUser>>();
            services.TryAddScoped<ITwoFactorSecurityStampValidator, TwoFactorSecurityStampValidator<TUser>>();
            services.TryAddScoped<IUserClaimsPrincipalFactory<TUser>, UserClaimsPrincipalFactory<TUser, TRole>>();
            services.TryAddScoped<IUserConfirmation<TUser>, DefaultUserConfirmation<TUser>>();
          
            services.TryAddScoped<JwtUserManager<TUser>>();
            services.TryAddScoped<UserManager<TUser>>(); // Leave default UserManager for internal usage
            services.TryAddScoped<JwtSignInManager<TUser>>();
            services.TryAddScoped<SignInManager<TUser>>(); // Leave default SignInManager for internal usage
            services.TryAddScoped<RoleManager<TRole>>();

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            return new IdentityBuilder(typeof(TUser), typeof(TRole), services);
        }

        public static IdentityBuilder AddJwtEntityFrameworkStores<TUserAccessToken, TContext>(this IdentityBuilder builder)
            where TContext : DbContext
            where TUserAccessToken : class
        {
            AddStores(builder.Services, builder.UserType, builder.RoleType, typeof(TUserAccessToken), typeof(TContext));

            return builder;
        }

        private static void AddStores(IServiceCollection services, Type userType, Type roleType, Type userAccessTokenType, Type contextType)
        {
            var identityUserType = FindGenericBaseType(userType, typeof(IdentityUser<>));
            if (identityUserType == null)
            {
                throw new InvalidOperationException("Resoruces.NotIdentityUser");
            }

            var keyType = identityUserType.GenericTypeArguments[0];

            if (roleType != null)
            {
                var identityRoleType = FindGenericBaseType(roleType, typeof(IdentityRole<>));
                if (identityRoleType == null)
                {
                    throw new InvalidOperationException("Resources.NotIdentityRole");
                }

                Type? userStoreType;
                Type? roleStoreType;
                var identityContext = FindGenericBaseType(contextType, typeof(IdentityDbContext<,,,,,,,>));

                if (identityContext == null)
                {
                    // If its a custom DbContext, we can only add the default POCOs
                    userStoreType = typeof(JwtUserStore<,,,,>).MakeGenericType(userType, userAccessTokenType, roleType, contextType, keyType);
                    roleStoreType = typeof(RoleStore<,,>).MakeGenericType(roleType, contextType, keyType);
                }
                else
                {
                    userStoreType = typeof(JwtUserStore<,,,,,,,,,>).MakeGenericType(userType, userAccessTokenType, roleType, contextType,
                        identityContext.GenericTypeArguments[2],
                        identityContext.GenericTypeArguments[3],
                        identityContext.GenericTypeArguments[4],
                        identityContext.GenericTypeArguments[5],
                        identityContext.GenericTypeArguments[7],
                        identityContext.GenericTypeArguments[6]);
                    roleStoreType = typeof(RoleStore<,,,,>).MakeGenericType(roleType, contextType,
                        identityContext.GenericTypeArguments[2],
                        identityContext.GenericTypeArguments[4],
                        identityContext.GenericTypeArguments[6]);
                }
                services.TryAddScoped(typeof(IUserStore<>).MakeGenericType(userType), userStoreType);
                services.TryAddScoped(typeof(IRoleStore<>).MakeGenericType(roleType), roleStoreType);
            }
        }

        private static Type? FindGenericBaseType(Type currentType, Type genericBaseType)
        {
            var type = currentType;
            while (type != null)
            {
                var genericType = type.IsGenericType ? type.GetGenericTypeDefinition() : null;
                if (genericType != null && genericType == genericBaseType)
                {
                    return type;
                }
                type = type.BaseType;
            }
            return null;
        }
    }
}