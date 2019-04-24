using Cleaners.Web.Models.Users;
using System;
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

            model.Roles = await _selectListService.GetRolesAsync();
        }

        public async Task PrepareUpdateModel(UserUpdateModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            model.Roles = await _selectListService.GetRolesAsync();
        }
    }
}