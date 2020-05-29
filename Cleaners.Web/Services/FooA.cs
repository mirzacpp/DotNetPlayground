using Cleaners.DependencyInjection.Interfaces;
using System;

namespace Cleaners.Web.Services
{
    public class FooA : IFoo, IScopedDependency
    {
        public string Name => nameof(FooA);

        public void Introduce()
        {
            Console.WriteLine($"Hello from {nameof(FooA)}");
        }
    }
}