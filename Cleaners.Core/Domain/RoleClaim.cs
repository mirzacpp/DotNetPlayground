using Microsoft.AspNetCore.Identity;

namespace Cleaners.Core.Domain
{
    public class RoleClaim : IdentityRoleClaim<int>
    {
        public Role Role { get; set; }
    }
}