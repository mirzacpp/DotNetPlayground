using Cleaners.Core.Domain;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Cleaners.Services.Identity
{
    /// <summary>
    /// Implementation of Identity wrapper
    /// </summary>
    public class IdentityManagement : IIdentityManagement
    {
        private readonly UserManager<User> _userManager;

        public IdentityManagement(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public IdentityOptions Options => _userManager.Options;

        public async Task<User> FindByNameAsync(string userName)
            => await _userManager.FindByNameAsync(userName);
    }
}