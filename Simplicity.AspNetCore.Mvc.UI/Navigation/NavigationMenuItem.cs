using System.Collections.Generic;
using System.Linq;

namespace Simplicity.AspNetCore.Mvc.UI.Navigation
{
    public class NavigationMenuItem
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Icon { get; set; }
        public int Order { get; set; }
        public string Url { get; set; }
        public List<NavigationMenuItem> SubItems { get; set; }

        public bool HasSubItems => SubItems.Any();

        public NavigationMenuItem(string name, string displayName, string icon, int order, string url)
        {
            Name = name;
            DisplayName = displayName;
            Icon = icon;
            Order = order;
            Url = url;

            SubItems = new List<NavigationMenuItem>();
        }

        public NavigationMenuItem AddSubItem(NavigationMenuItem item)
        {
            SubItems.Add(item);
            return this;
        }
    }
}