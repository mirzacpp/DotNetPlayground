using Cleaners.Web.Localization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Cleaners.Web.Models.Users
{
    public class UserResetPasswordModel
    {
        [HiddenInput]
        public int Id { get; set; }

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