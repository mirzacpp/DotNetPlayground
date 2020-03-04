using Cleaners.Services.Roles;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cleaners.Web.Services
{
    public class SelectListProviderService : ISelectListProviderService
    {
        private readonly IRoleService _roleService;
        private readonly IStringLocalizer<SelectListProviderService> _localizer;

        public SelectListProviderService(IRoleService roleService, IStringLocalizer<SelectListProviderService> localizer)
        {
            _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
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
            return (await _roleService.GetAllAsync())
                .Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name.ToString()
                })
                .ToList();
        }
    }
}