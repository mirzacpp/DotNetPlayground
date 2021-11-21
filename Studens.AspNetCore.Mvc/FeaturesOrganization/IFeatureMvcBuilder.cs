using Microsoft.Extensions.DependencyInjection;

namespace Studens.AspNetCore.Mvc.FeaturesOrganization;

/// <summary>
/// An interface for configuring Features services.
/// </summary>
public interface IFeatureMvcBuilder
{
    /// <summary>
    /// Gets the <see cref="IServiceCollection"/> where MiniProfiler services are configured.
    /// </summary>
    IServiceCollection Services { get; }
}
