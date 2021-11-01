using Microsoft.AspNetCore.Mvc;

namespace Weakling.WebUI.Features.Users
{
    public class UserController : Controller
    {
        public IActionResult Index() => View();
    }
}