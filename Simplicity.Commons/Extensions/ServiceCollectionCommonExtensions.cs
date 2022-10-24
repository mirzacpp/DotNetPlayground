namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Common registration extension methods for <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionCommonExtensions
{
    public static bool IsAdded(this IServiceCollection services, Type type) =>
        services.Any(p => p.ServiceType == type);

    public static bool IsAdded<T>(this IServiceCollection services) =>
        services.IsAdded(typeof(T));

    public static Lazy<T> GetServiceLazy<T>(this IServiceProvider serviceProvider) =>
        new(() => serviceProvider.GetService<T>(), isThreadSafe: true);

    public static Lazy<T> GetRequiredServiceLazy<T>(this IServiceProvider serviceProvider) =>
        new(() => serviceProvider.GetRequiredService<T>(), isThreadSafe: true);
}
