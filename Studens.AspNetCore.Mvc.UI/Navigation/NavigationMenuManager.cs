using Studens.AspNetCore.Mvc.UI.Navigation;
using System;
using System.Collections.Generic;

namespace Studens.AspNetCore.Mvc.UI.Navigation
{
	public class NavigationMenuManager : INavigationMenuManager
    {
        public IDictionary<string, NavigationMenu> Menus { get; private set; }

        public NavigationMenuManager()
        {
            Menus = new Dictionary<string, NavigationMenu>();
        }

        public NavigationMenu GetMenu(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Menu name cannot be null or empty.", nameof(name));
            }

            return Menus[name];
        }

        public void AddMenu(string name, NavigationMenu navigationMenu)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Menu name cannot be null or empty.", nameof(name));
            }

            if (navigationMenu is null)
            {
                throw new ArgumentNullException(nameof(navigationMenu));
            }

            Menus[name] = navigationMenu;
        }
    }
}