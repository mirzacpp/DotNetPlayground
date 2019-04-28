using Cleaners.Web.Localization;
using System.ComponentModel.DataAnnotations;

namespace Cleaners.Web.Models.Account
{
    public class AccountChangePasswordModel
    {
        [DataType(DataType.Password)]
        [Display(Name = ResourceKeys.CurrentPassword)]
        [Required(ErrorMessage = ResourceKeys.RequiredField)]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = ResourceKeys.NewPassword)]
        [Required(ErrorMessage = ResourceKeys.RequiredField)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = ResourceKeys.PasswordConfirmed)]
        [Required(ErrorMessage = ResourceKeys.RequiredField)]
        [Compare(nameof(NewPassword), ErrorMessage = ResourceKeys.PasswordMismatch)]
        public string PasswordConfirmed { get; set; }
    }
}