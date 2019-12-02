using Cleaners.Utils;
using Cleaners.Web.Constants;
using Corvo.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Cleaners.Web.Controllers
{
    [Authorize]
    [Route("")]
    public class HomeController : Controller
    {
        [Route("", Name = HomeRoutes.Index)]
        public IActionResult Index() => View();

        [HttpGet]
        public IActionResult Test()
        {
            var values = EnumUtils.GetEnumNames<DayOfWeek>().ToList();
            var values2 = EnumUtils.GetEnumValues<DayOfWeek>().ToList();

            return Content("");
        }

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

        [Route("about", Name = HomeRoutes.About)]
        public IActionResult About() => View();
    }
}