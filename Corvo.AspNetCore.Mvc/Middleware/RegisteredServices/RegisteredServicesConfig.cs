using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Corvo.AspNetCore.Mvc.Middleware.RegisteredServices
{
    /// <see cref="https://github.com/ardalis/AspNetCoreStartupServices"></see>
    public class RegisteredServicesConfig
    {
        public RegisteredServicesConfig()
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

        /// <summary>
        /// 
        /// </summary>
        public string Test { get; set; }
    }
}