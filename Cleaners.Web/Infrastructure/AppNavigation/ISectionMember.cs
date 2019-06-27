using System.Collections.Generic;

namespace Cleaners.Web.Infrastructure.AppNavigation
{
    /// <summary>
    /// Base interface for section providers
    /// </summary>
    /// <typeparam name="T">Section type</typeparam>
    public interface ISectionMember<T> where T : IComparer<T>
    {
        T Section { get; set; }
        bool IsAvailable { get; set; }
    }
}