﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Studens.AspNetCore.Identity;
using Studens.AspNetCore.Mvc.Extensions;
using Studens.MvcNet6.WebUI.Models;
using System.Diagnostics;

namespace Studens.MvcNet6.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IdentityUserManager<IdentityUser> _userManager;
        private readonly IdentityRoleManager<IdentityRole> _roleManager;
        private readonly IdentityPasswordManager _identityPasswordManager;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger,
            IdentityUserManager<IdentityUser> userManager,
            IdentityRoleManager<IdentityRole> roleManager,
            IdentityPasswordManager identityPasswordManager)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _identityPasswordManager = identityPasswordManager;
        }

        public async Task<IActionResult> Index()
        {
            var password = _identityPasswordManager.GenerateRandomPassword();
            var passwordStr = _identityPasswordManager.GetPasswordStrength(password);
            var ajax = HttpContext.Request.IsAjaxRequest();

            return Ok(ajax.ToString());
        }

        public IActionResult Privacy()
        {
            //_userManager.Addto()

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}