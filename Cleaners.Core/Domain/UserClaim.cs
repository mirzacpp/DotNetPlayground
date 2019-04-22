using Microsoft.AspNetCore.Identity;

namespace Cleaners.Core.Domain
{
    public class UserClaim : IdentityUserClaim<int>
    {
        public User User { get; set; }
    }
}