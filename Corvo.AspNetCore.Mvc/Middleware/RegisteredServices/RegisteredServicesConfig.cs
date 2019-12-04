using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Corvo.AspNetCore.Mvc.Middleware.RegisteredServices
{
    /// <see cref="https://github.com/ardalis/AspNetCoreStartupServices"></see>
    public class RegisteredServicesConfig
    {
        public RegisteredServicesConfig()
        {
            Path = "/registered-services";
            Services = new List<ServiceDescriptor>();
        }

        /// <summary>
        /// Path on which middleware will response
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// List of all entries inside <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection"/>
        /// </summary>
        public List<ServiceDescriptor> Services { get; set; }
    }
}