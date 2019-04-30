using Cleaners.Core.Domain;
using Cleaners.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cleaners.Services.Roles
{
    public class RoleService : IRoleService
    {
        private readonly IRepository _repository;
        private readonly RoleManager<Role> _roleManager;

        public RoleService(IRepository repository, RoleManager<Role> roleManager)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        public async Task<IdentityResult> CreateAsync(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            return await _roleManager.CreateAsync(role);
        }

        public async Task<IdentityResult> DeleteAsync(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            return await _roleManager.DeleteAsync(role);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await GetByIdAsync(id) != null;
        }

        public IEnumerable<Role> GetAll()
        {
            return _roleManager.Roles.ToList();
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<Role> GetByIdAsync(int id)
        {
            return await _roleManager.FindByIdAsync(id.ToString());
        }

        public async Task<IdentityResult> UpdateAsync(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            return await _roleManager.UpdateAsync(role);
        }
    }
}