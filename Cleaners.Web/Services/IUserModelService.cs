using Cleaners.Web.Models.Users;
using System.Threading.Tasks;

namespace Cleaners.Web.Services
{
    /// <summary>
    /// Common user model preparation methods
    /// </summary>
    public interface IUserModelService
    {
        Task PrepareModel(UserCreateModel model);
    }
}