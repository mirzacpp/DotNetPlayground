using Cleaners.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Cleaners.Core.Domain
{
    public class RoleClaim : IdentityRoleClaim<int>, IEntity
    {
        public Role Role { get; set; }
    }
}