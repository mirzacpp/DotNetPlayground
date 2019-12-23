using Corvo.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Cleaners.Extensions;
using System.Collections.Concurrent;

namespace Cleaners.Web.Controllers
{
    public class Tester
    {
        public string Id { get; set; }
        public List<string> Docs { get; }
        public Tester(string id, List<string> docs)
            => (Id, Docs) = (id, docs);

        public Tester()
        {
            Docs = new List<string>();
        }
    }

    [Authorize]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index() => View();

        [HttpGet]
        public IActionResult Test()
        {
            var docs = new[] { "Privates", "Shared" };

            var user = new Tester
            {
                Id = string.Empty,
                Docs = { docs.Where(doc => doc == "Shared") }
            };

            bool hasBooks = user.Docs?.Any() ?? false;

            return Content(string.Join(", ", user.Docs));
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
            1.ToString();
            return Json($"Action: {nameof(Submit2)}, with param {value}");
        }

        public IActionResult About() => View();
    }
}