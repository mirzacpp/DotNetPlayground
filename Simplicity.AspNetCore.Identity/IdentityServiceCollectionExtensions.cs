namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Contains extension methods to <see cref="IServiceCollection"/> for configuring identity services.
    /// </summary>
    public static class StudensIdentityServiceCollectionExtensions
    {
        public static IdentityBuilder AddStudensIdentity<TUser, TRole>(
            this IServiceCollection services)
            where TUser : class
            where TRole : class
            => services.AddStudensIdentity<TUser, TRole>(setupAction: null);

        public static IdentityBuilder AddStudensIdentity<TUser, TRole>(
            this IServiceCollection services,
            Action<IdentityOptions> setupAction)
            where TUser : class
            where TRole : class
        {
            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            return new IdentityBuilder(typeof(TUser), typeof(TRole), services);
        }
    }
}