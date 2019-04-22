using Microsoft.AspNetCore.Mvc;

namespace Cleaners.Web.Views.Shared.Components.NavigationMenu
{
    public class NavigationMenu : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("_NavigationMenu");
        }
    }
}