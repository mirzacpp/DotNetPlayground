using Cleaners.Web.Constants;
using Cleaners.Web.Infrastructure.Alerts;
using Cleaners.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace Cleaners.Web.Controllers
{
    [Authorize]
    [Route("")]
    public class HomeController : Controller
    {
        private readonly IAlertManager _alertManager;
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly IHostingEnvironment _hostingEnvironment;

        public HomeController(IAlertManager alertManager, IStringLocalizer<HomeController> localizer, IHostingEnvironment hostingEnvironment)
        {
            _alertManager = alertManager;
            _localizer = localizer;
            _hostingEnvironment = hostingEnvironment;
        }

        [Route("", Name = HomeRoutes.Index)]
        public IActionResult Index()
        {
            return View();
        }

        [Route("test")]
        public IActionResult Test()
        {
            return Content(_hostingEnvironment.ContentRootPath);

            //return View(model: new TestModel());
        }

        [HttpPost("test")]
        public IActionResult Test(TestModel model)
        {
            var culture = CultureInfo.CurrentCulture;
            var culture2 = CultureInfo.CurrentUICulture;
            if (!ModelState.IsValid)
            {
                return View(model: model);
            }

            return Json(model.Number);
            return View(model: model);
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}