using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace Studens.MvcNet6.WebUI.Areas.Auth.Models;

public class LoginViewModel
{
    public LoginViewModel()
    {
        ExternalLogins = Array.Empty<AuthenticationScheme>();
    }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }

    public IList<AuthenticationScheme> ExternalLogins { get; set; }
}