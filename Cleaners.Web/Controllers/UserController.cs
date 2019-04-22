using Cleaners.Core.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Cleaners.Web.Controllers
{
    /// <summary>
    /// Handles all requests for user
    /// </summary>
    [Route("users")]
    public class UserController : Controller
    {
        private readonly UserManager<User> userManager;

        public UserController(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public IActionResult Index() => View();

        //[HttpGet("create")]
        //public IActionResult Create()
        //{
        //    var res = userManager.CreateAsync(new User
        //    {
        //        Email = "mirza@ito.ba",
        //        UserName = "mirza@ito.ba",
        //        FirstName = "Miki",
        //        LastName = "Lauda",
        //        IsActive = true,
        //        IsDeleted = false,
        //        EmailConfirmed = true,
        //        CreationDateUtc = DateTime.UtcNow,                
        //    }, "Pass123$").Result;

        //    if (res.Succeeded)
        //    {
        //        return Content("Ok");
        //    }

        //    return Content(string.Join(", ", res.Errors.Select(e => e.Description)));
        //}
    }
}