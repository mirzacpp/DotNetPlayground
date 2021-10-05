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

    public class Repo2 : IRepo<Country>, IScopeDependency
    {
        public void Introduce()
        {
            Console.WriteLine($"Repo for type {typeof(Country).FullName}");
        }
    }    

    public class Repo3<T> : ITransientDependency where T : class
    {
        public string WhoAmI()
        {
            return typeof(T).ToString();
        }
    }  
}