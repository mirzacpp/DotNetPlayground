using Microsoft.AspNetCore.Mvc;

namespace Cleaners.Web.Models.Roles
{
    public class RoleConfirmationModel
    {
        [HiddenInput]
        public int Id { get; set; }
    }
}