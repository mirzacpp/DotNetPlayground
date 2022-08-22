namespace Studens.AspNetCore.Mvc.UI.Navigation
{
	public interface INavigationMenuManager
	{
		/// <summary>
		/// Holds all application defined menus
		/// </summary>
		IDictionary<string, NavigationMenu> Menus { get; }

		NavigationMenu GetMenu(string name);

		void AddMenu(string name, NavigationMenu navigationMenu);
	}
}