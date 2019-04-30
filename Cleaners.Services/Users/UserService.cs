using Cleaners.Core.Domain;
using Cleaners.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cleaners.Services.Users
{
    /// <summary>
    /// User service interface implementation
    /// </summary>
    public class UserService : IUserService
    {
        #region Fields

        private readonly IRepository _repository;

        /// <summary>
        /// We will user already existing User methods defined in UserManager when appropriate
        /// </summary>
        private readonly UserManager<User> _userManager;

        #endregion Fields

        public UserService(IRepository repository, UserManager<User> userManager)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            // Generate token for email confirmation.
            // Same tokens are send to users(Part of confirmation URI) when confirmation is their responsibility
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<IdentityResult> CreateAsync(User user, string password)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            // Since we don' use IRepository.Create, we will set creation date manually
            user.CreationDateUtc = DateTime.UtcNow;
            // Email confirmation is only required when client requests for new account
            user.EmailConfirmed = true;

            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> UpdateAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            // Since we don' use IRepository.Update, we will set update date manually
            user.LastUpdateDateUtc = DateTime.UtcNow;

            return await _userManager.UpdateAsync(user);
        }

        public IEnumerable<User> Get()
        {
            return _userManager.Users.ToList();
        }

        public async Task<IEnumerable<User>> GetAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            // There is no UserManager<TUser, TKey> so we will have to convert value to string.
            // This is ok because ids will be converted to string anyway
            return await _userManager.FindByIdAsync(id.ToString());
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await GetByIdAsync(id) != null;
        }

        public async Task<IdentityResult> AddToRolesAsync(User user, IEnumerable<string> roles)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (roles == null)
            {
                throw new ArgumentNullException(nameof(roles));
            }

            return await _userManager.AddToRolesAsync(user, roles);
        }

        public async Task<IEnumerable<string>> GetRolesAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return await _userManager.GetRolesAsync(user);
        }

        public async Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrEmpty(currentPassword))
            {
                throw new ArgumentException(nameof(currentPassword));
            }

            if (string.IsNullOrEmpty(newPassword))
            {
                throw new ArgumentException(nameof(newPassword));
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);            
        }

        public async Task<IdentityResult> DeleteAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            // Since we don' use IRepository.SoftDelete, we will do it manually
            user.IsDeleted = true;

            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> RestoreAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            // Since we don' use IRepository.Restore, we will do it manually
            user.IsDeleted = false;

            return await _userManager.UpdateAsync(user);
        }
    }
}