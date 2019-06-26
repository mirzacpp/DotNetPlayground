using Microsoft.AspNetCore.Mvc;

namespace Cleaners.Web.Controllers
{
    [Route("signal-r")]
    public class SignalRController : CorvoControllerBase
    {
        public IActionResult Index() => View();
    }
}