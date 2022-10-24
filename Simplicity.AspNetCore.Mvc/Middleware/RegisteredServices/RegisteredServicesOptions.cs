using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Simplicity.AspNetCore.Mvc.Middleware.RegisteredServices;

public class RegisteredServicesOptions
{
    public RegisteredServicesOptions()
    {
        DefaultPath = new PathString("/registered-services");
        Services = new List<ServiceDescriptor>();
    }

    /// <summary>
    /// Path on which middleware will response
    /// </summary>
    public PathString DefaultPath { get; set; }

    /// <summary>
    /// List of all entries inside <see cref="IServiceCollection"/>
    /// </summary>
    public IList<ServiceDescriptor> Services { get; set; }
}