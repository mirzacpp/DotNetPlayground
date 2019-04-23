using Cleaners.Core.Domain;
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
    }
}