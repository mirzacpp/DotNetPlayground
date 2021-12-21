using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Studens.AspNetCore.Identity;
using Studens.MvcNet6.WebUI.Models;
using System.Diagnostics;

namespace Studens.MvcNet6.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IdentityUserManager<IdentityUser> _userManager;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IdentityUserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.GetAllAsync();
            return Ok(users);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}