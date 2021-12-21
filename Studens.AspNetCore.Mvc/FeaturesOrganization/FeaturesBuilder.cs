using Microsoft.Extensions.DependencyInjection;

namespace Studens.AspNetCore.Mvc.FeaturesOrganization;

/// <summary>
/// Allows fine grained configuration of Features services.
/// </summary>
public class FeaturesBuilder
{
    /// <summary>
    /// Initializes a new <see cref="FeaturesBuilder"/> instance.
    /// </summary>
    /// <param name="mvcBuilder">Current mvc builder</param>
    /// <exception cref="ArgumentNullException">Throws when <paramref name="services"/> is null.</exception>
    public FeaturesBuilder(IMvcBuilder mvcBuilder)
    {
        MvcBuilder = mvcBuilder ?? throw new ArgumentNullException(nameof(mvcBuilder));
    }

    /// <summary>
    /// Current mvc builder instance
    /// </summary>
    public IMvcBuilder MvcBuilder { get; }
}