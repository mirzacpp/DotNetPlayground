using System;

namespace Cleaners.Web.Services
{
    public class FooA : IFoo
    {
        public string Name => nameof(FooA);

        public void Introduce()
        {
            Console.WriteLine($"Hello from {nameof(FooA)}");
        }
    }
}