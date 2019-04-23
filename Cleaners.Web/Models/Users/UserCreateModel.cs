using Cleaners.Web.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cleaners.Web.Models.Users
{
    public class UserCreateModel
    {
        [Display(Name = ResourceKeys.FirstName)]
        [Required(ErrorMessage = ResourceKeys.RequiredField)]
        public string FirstName { get; set; }

        [Display(Name = ResourceKeys.LastName)]
        [Required(ErrorMessage = ResourceKeys.RequiredField)]
        public string LastName { get; set; }

        [Display(Name = ResourceKeys.Email)]
        [Required(ErrorMessage = ResourceKeys.RequiredField)]
        public string Email { get; set; }

        [Display(Name = ResourceKeys.UserName)]
        [Required(ErrorMessage = ResourceKeys.RequiredField)]
        public string UserName { get; set; }

        [Display(Name = ResourceKeys.Password)]
        [Required(ErrorMessage = ResourceKeys.RequiredField)]
        public string Password { get; set; }

        [Display(Name = ResourceKeys.PasswordConfirmed)]
        [Required(ErrorMessage = ResourceKeys.RequiredField)]
        public string PasswordConfirmed { get; set; }

        [Display(Name = ResourceKeys.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = ResourceKeys.LockoutEnabled)]
        public bool LockoutEnabled { get; set; }

        [Display(Name = ResourceKeys.Active)]
        public bool IsActive { get; set; }

        [Display(Name = ResourceKeys.Roles)]
        [Required(ErrorMessage = ResourceKeys.RequiredField)]
        public IEnumerable<int> RoleIds { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}