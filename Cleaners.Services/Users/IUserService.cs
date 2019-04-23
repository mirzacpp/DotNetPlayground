using Cleaners.Core.Domain;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cleaners.Services.Users
{
    /// <summary>
    /// User based interface
    /// </summary>
    public interface IUserService
    {
        IEnumerable<User> Get();

        Task<IEnumerable<User>> GetAsync();

        Task<IdentityResult> CreateAsync(User user, string password);

        Task<IdentityResult> UpdateAsync(User user);

        Task<IdentityResult> ConfirmEmailAsync(User user);
    }
}