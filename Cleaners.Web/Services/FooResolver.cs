using System;
using System.Collections.Generic;
using System.Linq;

namespace Cleaners.Web.Services
{
    public class FooResolver : IFooResolver
    {
        /// <summary>
        /// All <see cref="IFoo"/> implementations
        /// </summary>
        private readonly IEnumerable<IFoo> _foos;

        public FooResolver(IEnumerable<IFoo> foos)
        {
            _foos = foos;
        }

        public IFoo GetInstance(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name", nameof(name));
            }

            return _foos.FirstOrDefault(f => f.Name == name);
        }
    }
}