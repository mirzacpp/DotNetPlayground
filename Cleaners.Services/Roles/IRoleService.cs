using Cleaners.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cleaners.Services.Roles
{
    public interface IRoleService
    {
        IEnumerable<Role> GetAll();

        Task<IEnumerable<Role>> GetAllAsync();
    }
}