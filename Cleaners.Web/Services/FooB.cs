using Cleaners.DependencyInjection.Interfaces;
using System;

namespace Cleaners.Web.Services
{
    public class FooB : IFoo, IScopedDependency
    {
        public string Name => nameof(FooB);

        public void Introduce()
        {
            Console.WriteLine($"Hello from {nameof(FooB)}");
        }
    }
}