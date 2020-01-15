using Corvo.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cleaners.Web.Controllers
{
    [Authorize]
    public class HomeController : CorvoControllerBase
    {
        [HttpGet]
        public IActionResult Index() => View();

        [HttpGet]
        public IActionResult Test()
            => AjaxRedirectToActionResult(nameof(Submit));

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

        public IActionResult About() => View();

        [HttpGet]
        public IActionResult AjaxTest() => View();

        public IActionResult AjaxTestContent() => PartialView("_AjaxTestContent");

        public IActionResult AjaxTestContent2() => Content(Url.Action(nameof(AjaxTest)));

        //public IActionResult AjaxTestContent2() => Json(new { redirectUrl = Url.Action(nameof(AjaxTest)) });
    }
}