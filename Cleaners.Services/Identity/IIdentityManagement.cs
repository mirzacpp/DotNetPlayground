using Cleaners.Core.Domain;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Cleaners.Services.Identity
{
    /// <summary>
    /// Wraps all abstractions for AspNetCore Identity
    /// </summary>
    public interface IIdentityManagement
    {
        IdentityOptions Options { get; }

        Task<User> FindByNameAsync(string userName);
    }
}