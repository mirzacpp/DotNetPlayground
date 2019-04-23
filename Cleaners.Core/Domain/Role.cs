using Cleaners.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Cleaners.Core.Domain
{
    public class Role : IdentityRole<int>, IEntity
    {
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<RoleClaim> RoleClaims { get; set; }
    }
}