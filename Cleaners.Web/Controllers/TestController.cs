using Microsoft.AspNetCore.Mvc;

namespace Cleaners.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class TestController : Controller
    {
        [HttpGet("{param}", Order = -5)]
        public IActionResult CallMe(int param)
        {
            return Content($"Hello from {nameof(CallMe)} with param {nameof(param)} and value {param}");
        }

        [HttpGet("{ok2}")]
        public IActionResult CallMe(string ok2, string ok)
        {
            return Content($"Hello from {nameof(CallMe)} with param {nameof(ok)} and value {ok}/{ok2}");
        }
    }
}