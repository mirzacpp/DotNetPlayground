namespace Cleaners.Web.Localization
{
    /// <summary>
    /// Represents available language in application
    /// </summary>
    public class Language
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsDefault { get; set; }
        public bool IsDisabled { get; set; }
        public string Icon { get; set; }

        public Language(string name, string displayName, bool isDefault = false, bool isDisabled = false, string icon = null)
        {
            Name = name;
            DisplayName = displayName;
            IsDefault = isDefault;
            IsDisabled = isDisabled;
            Icon = icon;
        }
    }
}