using Microsoft.AspNetCore.Mvc;

namespace Cleaners.Web.Models.Users
{
    public class UserConfirmationModel
    {
        [HiddenInput]
        public int Id { get; set; }
    }
}