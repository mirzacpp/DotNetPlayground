using Microsoft.Extensions.DependencyInjection;

namespace Studens.AspNetCore.Mvc.FeaturesOrganization;

/// <summary>
/// Allows fine grained configuration of Features services.
/// </summary>
public class FeatureMvcBuilder : IFeatureMvcBuilder
{
    /// <summary>
    /// Initializes a new <see cref="FeatureMvcBuilder"/> instance.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <exception cref="ArgumentNullException">Throws when <paramref name="services"/> is null.</exception>
    public FeatureMvcBuilder(IServiceCollection services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    /// <summary>
    /// Services for this <see cref="FeatureMvcBuilder"/>.
    /// </summary>
    public IServiceCollection Services { get; }
}
