/// <summary>
/// Extension methods for <see cref="IServiceCollection"/>
/// </summary>
namespace Microsoft.Extensions.DependencyInjection;

public static partial class CommonServiceCollectionExtensions
{
    public static IServiceCollection AddIf(this IServiceCollection services,
        bool predicate,
        Func<IServiceCollection> compose) 
        => predicate ? compose() : services;
}
