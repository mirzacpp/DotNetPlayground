using AutoMapper;
using Cleaners.Core.Domain;
using Cleaners.Web.Models.Users;

namespace Cleaners.Web.Infrastructure.AutoMapper
{
    /// <summary>
    /// Configures necessary mappings
    /// </summary>
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            #region User

            CreateMap<User, UserModel>();

            #endregion User
        }
    }
}