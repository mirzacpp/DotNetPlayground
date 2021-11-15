using Microsoft.AspNetCore.Identity;

namespace Cleaners.Core.Domain
{
    public class UserLogin : IdentityUserLogin<int>
    {
        public User User { get; set; }
    }
}