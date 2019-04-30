using Cleaners.Web.Models.Users;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cleaners.Web.Services
{
    /// <summary>
    /// Common user model preparation methods
    /// </summary>
    public class UserModelService : IUserModelService
    {
        private readonly ISelectListProviderService _selectListService;

        public UserModelService(ISelectListProviderService selectListService)
        {
            _selectListService = selectListService ?? throw new ArgumentNullException(nameof(selectListService));
        }

        public async Task PrepareCreateModel(UserCreateModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            model.Roles = await _selectListService.GetRolesWithNamesAsync();
        }

        public async Task PrepareUpdateModel(UserUpdateModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            model.Roles = await _selectListService.GetRolesWithNamesAsync();

            // Since select2 doesn't preselect selected values, we will do that manually (Check Update.cshtml)
            // Take only roles that are not set for user
            model.Roles = model.Roles.Where(r => !model.SelectedRoles.Contains(r.Value)).ToList();
        }
    }
}