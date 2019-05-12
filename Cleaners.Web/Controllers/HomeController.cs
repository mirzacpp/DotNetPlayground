using Cleaners.Web.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cleaners.Web.Controllers
{
    [Authorize]
    [Route("")]
    public class HomeController : Controller
    {
        [Route("", Name = HomeRoutes.Index)]
        public IActionResult Index() => View();

        [Route("about", Name = HomeRoutes.About)]
        public IActionResult About() => View();
    }
}