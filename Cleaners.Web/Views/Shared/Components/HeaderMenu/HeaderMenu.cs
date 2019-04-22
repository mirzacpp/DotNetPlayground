using Microsoft.AspNetCore.Mvc;

namespace Cleaners.Web.Views.Shared.Components.HeaderMenu
{
    /// <summary>
    /// Renders header menu bar
    /// </summary>
    public class HeaderMenu : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("_HeaderMenu");
        }
    }
}