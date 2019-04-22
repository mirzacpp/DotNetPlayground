using Microsoft.AspNetCore.Identity;

namespace Cleaners.Core.Domain
{
    public class UserToken : IdentityUserToken<int>
    {
        public User User { get; set; }
    }
}