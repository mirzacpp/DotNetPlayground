using AutoMapper;
using Cleaners.Core.Domain;
using Cleaners.Web.Models.Roles;
using Cleaners.Web.Models.Users;

namespace Cleaners.Web.Infrastructure.AutoMapper
{
    /// <summary>
    /// Configures necessary mappings
    /// </summary>
    public class AutoMapperConfig : Profile
    {
        /// <summary>
        /// Create separate profile for all entities
        /// </summary>
        public AutoMapperConfig()
        {
            #region User

            CreateMap<User, UserModel>();
            CreateMap<UserCreateModel, User>();
            CreateMap<UserUpdateModel, User>();
            CreateMap<User, UserUpdateModel>();
            CreateMap<User, UserDetailsModel>();

            #endregion User

            #region Role

            CreateMap<Role, RoleModel>();
            CreateMap<RoleCreateModel, Role>();
            CreateMap<RoleUpdateModel, Role>();
            CreateMap<Role, RoleUpdateModel>();

            #endregion Role
        }
    }
}