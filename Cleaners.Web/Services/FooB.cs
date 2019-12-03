using System;

namespace Cleaners.Web.Services
{
    public class FooB : IFoo
    {
        public string Name => nameof(FooB);

        public void Introduce()
        {
            Console.WriteLine($"Hello from {nameof(FooB)}");
        }
    }
}