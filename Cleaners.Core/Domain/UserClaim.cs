using Cleaners.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Cleaners.Core.Domain
{
    public class UserClaim : IdentityUserClaim<int>, IEntity
    {
        public User User { get; set; }
    }
}