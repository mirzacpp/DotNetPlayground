using Cleaners.Web.Localization;
using System.ComponentModel.DataAnnotations;

namespace Cleaners.Web.Models.Account
{
    public class LoginModel
    {
        [Required(ErrorMessage = ResourceKeys.RequiredField)]
        [Display(Prompt = ResourceKeys.UserName)]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Display(Prompt = ResourceKeys.Password)]
        [Required(ErrorMessage = ResourceKeys.RequiredField)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}