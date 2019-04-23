using Cleaners.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Cleaners.Core.Domain
{
    public class User :
        IdentityUser<int>, IEntity, ISoftDeletableEntity, IAuditableEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreationDateUtc { get; set; }
        public DateTime? LastUpdateDateUtc { get; set; }

        public ICollection<UserClaim> Claims { get; set; }
        public ICollection<UserLogin> Logins { get; set; }
        public ICollection<UserToken> Tokens { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}