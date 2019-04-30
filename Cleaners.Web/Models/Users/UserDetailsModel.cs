using Cleaners.Web.Localization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cleaners.Web.Models.Users
{
    public class UserDetailsModel
    {
        [Display(Name = ResourceKeys.FirstName)]
        public string FirstName { get; set; }

        [Display(Name = ResourceKeys.FirstName)]
        public string LastName { get; set; }

        [Display(Name = ResourceKeys.Email)]
        public string Email { get; set; }

        [Display(Name = ResourceKeys.EmailConfirmed)]
        public bool EmailConfirmed { get; set; }

        [Display(Name = ResourceKeys.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = ResourceKeys.LockoutEnabled)]
        public bool LockoutEnabled { get; set; }

        [Display(Name = ResourceKeys.AccessFailedCount)]
        public string AccessFailedCount { get; set; }

        [Display(Name = ResourceKeys.Active)]
        public bool IsActive { get; set; }

        [Display(Name = ResourceKeys.CreationDate)]
        public string CreationDateUtc { get; set; }

        [Display(Name = ResourceKeys.LastUpdateDate)]
        public string LastUpdateDateUtc { get; set; }

        [Display(Name = ResourceKeys.LockoutEnd)]
        public string LockoutEnd { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}