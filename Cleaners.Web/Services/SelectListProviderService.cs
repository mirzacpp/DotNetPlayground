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

        public IEnumerable<SelectListItem> GetRolesWithNames()
        {
            return _roleService.GetAll()
                .Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name
                })
                .ToList();
        }

        public async Task<IEnumerable<SelectListItem>> GetRolesWithNamesAsync()
        {
            var roles = await _roleService.GetAllAsync();

            return roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name.ToString()
            })
            .ToList();
        }
    }
}