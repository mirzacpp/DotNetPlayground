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

        Task<User> GetByIdAsync(int id);

        Task<bool> ExistsAsync(int id);

        Task<IEnumerable<string>> GetRolesAsync(User user);

        Task<IEnumerable<User>> GetAsync();

        Task<IdentityResult> CreateAsync(User user, string password);

        Task<IdentityResult> UpdateAsync(User user);

        Task<IdentityResult> ConfirmEmailAsync(User user);

        Task<IdentityResult> AddToRolesAsync(User user, IEnumerable<string> roles);

        Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword);
    }
}