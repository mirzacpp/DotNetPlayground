using Microsoft.Extensions.DependencyInjection;
using System;

namespace Cleaners.Web.Services.TestDI
{
    public class MyServiceDescriptor : ServiceDescriptor
    {
        public string ImplementationName { get; set; }

        public MyServiceDescriptor(Type serviceType, object instance, string implementationName)
            : base(serviceType, instance)
        {
            ImplementationName = implementationName;
        }

        public MyServiceDescriptor(Type serviceType, Type implementationType, ServiceLifetime lifetime, string implementationName)
            : base(serviceType, implementationType, lifetime)
        {
            ImplementationName = implementationName;
        }

        public MyServiceDescriptor(Type serviceType, Func<IServiceProvider, object> factory, ServiceLifetime lifetime, string implementationName)
            : base(serviceType, factory, lifetime)
        {
            ImplementationName = implementationName;
        }
    }
}