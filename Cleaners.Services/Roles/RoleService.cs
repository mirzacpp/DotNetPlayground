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

        public IEnumerable<Role> GetAll()
        {
            return _roleManager.Roles.ToList();
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }
    }
}