using Cleaners.Models;
using Cleaners.Web.Extensions;
using Corvo.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authentication;
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
        {
            var result = Result.Failed(new[]
            {
                new ResultError(string.Empty, "Model error"),
                new ResultError("Property1", "Property1 error"),
                new ResultError("Property1", "Property1 error 2"),
                new ResultError("Property2", "Property2 error"),
            });            

            ModelState.AddResultErrors(result.Errors);

            return BadRequest(ModelState);
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

        public IActionResult About() => View();

        [HttpGet]
        public IActionResult AjaxTest() => View();

        public IActionResult AjaxTestContent() => PartialView("_AjaxTestContent");

        public IActionResult AjaxTestContent2() => Content(Url.Action(nameof(AjaxTest)));

        //public IActionResult AjaxTestContent2() => Json(new { redirectUrl = Url.Action(nameof(AjaxTest)) });
    }
}