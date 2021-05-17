using Cleaners.Core.DependencyInjection.Cleaners;

namespace Corvo.Consola.DI
{
    public class CityRepo : IRepo<City>, ISingletonDependency
    {
        public void Introduce()
        {
            System.Console.WriteLine("Inside city repo");
        }
    }
}