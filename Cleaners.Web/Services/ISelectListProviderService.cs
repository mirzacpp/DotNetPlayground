using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cleaners.Web.Services
{
    public interface ISelectListProviderService
    {
        IEnumerable<SelectListItem> GetRolesWithNames();

        Task<IEnumerable<SelectListItem>> GetRolesWithNamesAsync();

        IEnumerable<SelectListItem> GetForEnum<TEnum>() where TEnum : struct, IConvertible;
    }
}