using Cleaners.Core.DependencyInjection.Cleaners;
using System;

namespace Corvo.Consola.DI
{
    public class Repo<T> : IRepo<T>, ISingletonDependency
    {
        public void Introduce()
        {
            Console.WriteLine($"Repo for type {typeof(T).FullName}");
        }
    }

    public class Repo2 : IRepo<Country>, ISingletonDependency
    {
        public void Introduce()
        {
            Console.WriteLine($"Repo for type {typeof(Country).FullName}");
        }
    }
}