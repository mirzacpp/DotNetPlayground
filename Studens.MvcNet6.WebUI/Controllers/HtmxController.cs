using Microsoft.AspNetCore.Mvc;

namespace Studens.MvcNet6.WebUI.Controllers
{
    public class HtmxController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult HelloMessage() =>
            Content("Hello from Htmx controller");

        [HttpGet]
        public async Task<IActionResult> HelloMessageDelayed()
        {
            await Task.Delay(2000);

            return Content("Hello from Htmx controller with some delay.");
        }
    }
}