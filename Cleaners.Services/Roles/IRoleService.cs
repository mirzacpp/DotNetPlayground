using Cleaners.Core.Domain;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cleaners.Services.Roles
{
    public interface IRoleService
    {
        IEnumerable<Role> GetAll();

        Task<IEnumerable<Role>> GetAllAsync();

        Task<IdentityResult> CreateAsync(Role role);

        Task<Role> GetByIdAsync(int id);

        Task<IdentityResult> UpdateAsync(Role role);
    }
}