using Cleaners.Core.Domain;
using Cleaners.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
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

        public IEnumerable<User> Get()
        {
            return _repository.Get<User>();
        }

        public async Task<IEnumerable<User>> GetAsync()
        {
            return await _repository.GetAsync<User>();
        }
    }
}