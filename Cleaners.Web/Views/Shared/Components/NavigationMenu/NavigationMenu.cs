using Corvo.AspNetCore.Mvc.UI.Navigation;
using Microsoft.AspNetCore.Mvc;

namespace Cleaners.Web.Views.Shared.Components.NavigationMenu
{
    public class NavigationMenu : ViewComponent
    {
        private readonly INavigationMenuManager _navigationMenuManager;

        public NavigationMenu(INavigationMenuManager navigationMenuManager)
        {
            _navigationMenuManager = navigationMenuManager;
        }

        public IViewComponentResult Invoke()
        {
            return View("_NavigationMenu");
        }
    }
}