using Cleaners.Web.Constants;
using Corvo.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;

namespace Cleaners.Web.Controllers
{
    [Authorize]
    [Route("")]
    public class HomeController : Controller
    {
        [Route("", Name = HomeRoutes.Index)]
        public IActionResult Index() => View();

        [HttpPost]
        public IActionResult Submit(string value)
        {
            return Json($"Action: {nameof(Submit)}, with param {value}");
        }

        [HttpPost]
        [ActionName(nameof(Submit))]
        [FormValueRequired("submit2")]
        public IActionResult Submit2(string value)
        {            
            return Json($"Action: {nameof(Submit2)}, with param {value}");
        }

        [Route("test")]
        public IActionResult Test()
        {
            var cultureInfo = new CultureInfo("hr");

            return Content(DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));
            //return Content(string.Join(", ", cultureInfo.DateTimeFormat.MonthNames));
        }

        [Route("about", Name = HomeRoutes.About)]
        public IActionResult About() => View();
    }
}