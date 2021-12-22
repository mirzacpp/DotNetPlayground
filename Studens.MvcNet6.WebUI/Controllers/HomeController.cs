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
        private readonly IdentityRoleManager<IdentityRole> _roleManager;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IdentityUserManager<IdentityUser> userManager, IdentityRoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.GetAsync(skip: 0, take: 30);
            var roles = await _roleManager.GetAllAsync();
            return Ok(roles);
        }

        public IActionResult Privacy()
        {
            _userManager.Addto()

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}