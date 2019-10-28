using Cleaners.Utils;
using Cleaners.Web.Constants;
using Corvo.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Cleaners.Web.Controllers
{
    internal class ViewModel
    {
        public int Id { get; set; }
        public string Color { get; set; }
    }

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
            var curr = DateTime.UtcNow;
            var fut = curr.AddDays(3);
            var arr = DateTimeUtils.GetWeekDates(curr).ToList().Select(c => c.ToShortDateString());

            return Content(string.Join(", ", arr));
        }

        [Route("about", Name = HomeRoutes.About)]
        public IActionResult About() => View();
    }
}