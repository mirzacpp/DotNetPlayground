using Cleaners.Core.Domain;
using Microsoft.AspNetCore.Identity;
using System;
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

        Task<IdentityResult> DeleteAsync(User user);

        Task<IdentityResult> RestoreAsync(User user);

        Task<IdentityResult> ConfirmEmailAsync(User user);

        Task<IdentityResult> AddToRolesAsync(User user, IEnumerable<string> roles);

        Task<IdentityResult> RemoveFromRolesAsync(User user, IEnumerable<string> roles);

        Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword);

        /// <summary>
        /// This overload is used for internal password reset so we don't pass in reset token
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="newPassword">Password</param>
        /// <returns>Operation result</returns>
        Task<IdentityResult> ResetPasswordAsync(User user, string newPassword);

        Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword);

        Task<IdentityResult> LockAccount(User user, DateTime? lockoutEndUtc);
    }
}