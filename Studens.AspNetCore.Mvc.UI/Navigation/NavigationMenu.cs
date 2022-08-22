using System.Collections.Generic;
using System.Linq;

namespace Studens.AspNetCore.Mvc.UI.Navigation
{
    /// <summary>
    /// Navigation menu definition
    /// </summary>
    public class NavigationMenu
    {
        /// <summary>
        /// Unique name for menu
        /// </summary>
        public string Name { get; set; }

        public string DisplayName { get; set; }

        /// <summary>
        /// First level items
        /// </summary>
        public List<NavigationMenuItem> Items { get; set; }

        public bool HasItems => Items.Any();

        public NavigationMenu(string name, string displayName)
        {
            Name = name;
            DisplayName = displayName;
            Items = new List<NavigationMenuItem>();
        }

        public NavigationMenu AddNavigationItem(NavigationMenuItem item)
        {
            Items.Add(item);
            // Allow menu chaining on declaration
            return this;
        }
    }
}