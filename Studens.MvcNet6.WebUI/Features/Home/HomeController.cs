﻿using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Simplicity.AspNetCore.Identity;
using Simplicity.AspNetCore.Mvc.Filters;

namespace Simplicity.MvcNet6.WebUI.Features.Home
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

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _roleManager.GetAsync(0, 100, "Ad");
            return View("Index");
        }

        [HttpPost]
        [FormValueRequired(false, "miki", "miki2")]
        public async Task<IActionResult> IndexPost()
        {
            var form = Request.Form.Keys.ToList();

            return Ok(form);
        }

        [EnvironmentRequired("Development")]
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