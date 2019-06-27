using System.Collections.Generic;
using System.Linq;

namespace Cleaners.Web.Infrastructure.AppNavigation
{
    /// <summary>
    /// Default application section service implementation
    /// </summary>
    /// <remarks>
    /// Implementation is based on hardcoded section values
    /// Change behavior
    /// </remarks>
    public class DefaultApplicationSectionService<TSection> : IApplicationSectionService<TSection> where TSection : IComparer<TSection>
    {
        public bool CheckIfAvailable(TSection section)
        {
            //return Sections.Any(s => s.Equals<TSection>(section));
            return true;
        }

        public IEnumerable<SectionMemeber<TSection>> Sections { get; set; }
    }
}