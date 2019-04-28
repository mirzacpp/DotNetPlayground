using Cleaners.Web.Localization;
using System.ComponentModel.DataAnnotations;

namespace Cleaners.Web.Models.Roles
{
    public class RoleCreateModel
    {
        [Display(Name = ResourceKeys.Name)]
        [Required(ErrorMessage = ResourceKeys.RequiredField)]
        public string Name { get; set; }
    }
}