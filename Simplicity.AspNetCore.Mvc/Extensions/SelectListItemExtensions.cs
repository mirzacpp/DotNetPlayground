namespace Microsoft.AspNetCore.Mvc.Rendering;

/// <summary>
/// Extension methods for <see cref="SelectListItem"/>
/// </summary>
public static partial class SelectListItemExtensions
{
    /// <summary>
    /// Transforms given enum <paramref name="list"/> into list of <see cref="SelectListItem"/> items.
    /// </summary>
    /// <typeparam name="T">Type of the Enum</typeparam>
    /// <param name="list">List of Enums</param>
    /// <param name="textSelector">Func to access text property</param>
    /// <returns>Collection of SelectListItem</returns>
    public static IList<SelectListItem> ToSelectList<T>(this IList<T> list, Func<T, string> textSelector) where T : Enum =>
        list.Select(s => new SelectListItem(textSelector(s), s.ToString())).ToList();

    /// <summary>
    /// Transforms given <paramref name="list"/> into list of <see cref="SelectListItem"/> items.
    /// </summary>
    /// <typeparam name="T">Type of the list</typeparam>
    /// <typeparam name="TValueProperty">Type for the value property</typeparam>
    /// <param name="list">List of items</param>
    /// <param name="valueSelector"></param>
    /// <param name="valueSelector">Func to access value property</param>
    /// <param name="textSelector">Func to access text property</param>
    /// <returns>Collection of SelectListItem</returns>
    public static IList<SelectListItem> ToSelectList<T, TValueProperty>(this IList<T> list,
        Func<T, TValueProperty> valueSelector,
        Func<T, string> textSelector)
        where TValueProperty : IConvertible =>
        list.Select(s => new SelectListItem(textSelector(s), valueSelector(s).ToString())).ToList();

    /// <summary>
    /// Appends option to the <paramref name="items"/>
    /// </summary>
    /// <param name="items">List of items</param>
    /// <param name="text">Option text</param>
    /// <param name="value">Option value</param>
    /// <param name="markAsSelected">Flag to mark option as selected</param>
    /// <returns>Collection of SelectListItem</returns>
    public static IList<SelectListItem> AddOptionItem(this IList<SelectListItem> items,
        string text,
        string? value = null,
        bool markAsSelected = false) =>
        items.AddOptionItem(new SelectListItem(text, value ?? string.Empty, markAsSelected));

    /// <summary>
    /// Appends option to the <paramref name="items"/>
    /// </summary>
    /// <param name="item">Option to append</param>
    /// <returns>Collection of SelectListItem</returns>
    public static IList<SelectListItem> AddOptionItem(this IList<SelectListItem> items, SelectListItem item)
    {
        items.AddFirst(item);

        return items;
    }

    /// <summary>
    /// Preselects option with <paramref name="value"/>
    /// </summary>
    /// <param name="items">List of items</param>
    /// <param name="value">Option value</param>
    /// <returns>Collection of SelectListItem</returns>
    public static IList<SelectListItem> PreselectItem(this IList<SelectListItem> items, string? value = null)
    {
        var item = items.FirstOrDefault(p => p.Value == value);

        if (item != null)
        {
            item.Selected = true;
        }

        return items;
    }
}