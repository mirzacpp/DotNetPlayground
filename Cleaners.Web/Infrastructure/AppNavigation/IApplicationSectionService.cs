using System.Collections.Generic;

namespace Cleaners.Web.Infrastructure.AppNavigation
{
    /// <summary>
    /// Defines contract for applicatin sections status
    /// </summary>
    public interface IApplicationSectionService<TSection> where TSection : IComparer<TSection>
    {
        bool CheckIfAvailable(TSection section);
    }
}