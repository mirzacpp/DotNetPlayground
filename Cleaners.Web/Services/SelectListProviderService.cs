using Cleaners.Services.Roles;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cleaners.Web.Services
{
    public class SelectListProviderService : ISelectListProviderService
    {
        private readonly IRoleService _roleService;

        public SelectListProviderService(IRoleService roleService)
        {
            _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
        }

        public IEnumerable<SelectListItem> GetRoles()
        {
            return _roleService.GetAll()
                .Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Id.ToString()
                })
                .ToList();
        }

        public async Task<IEnumerable<SelectListItem>> GetRolesAsync()
        {
            var roles = await _roleService.GetAllAsync();

            return roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id.ToString()
            })
            .ToList();
        }
    }
}