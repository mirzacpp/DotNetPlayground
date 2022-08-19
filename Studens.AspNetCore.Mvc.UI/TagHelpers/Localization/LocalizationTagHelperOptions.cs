namespace Studens.AspNetCore.Mvc.UI.TagHelpers.Localization
{
	public class LocalizationTagHelperOptions
	{
		/// <summary>
		/// Configures UI framework to be used by tag helper.
		/// Defaults to <c>Bootstrap5</c>
		/// </summary>
		public string UIFramework { get; set; } = LocalizationTagHelperDefaults.UIFrameworkVersion.Bootstrap5;
	}

	public static class LocalizationTagHelperDefaults
	{
		/// <summary>
		/// Predefines default supported UI frameworks
		/// </summary>
		public static class UIFrameworkVersion
		{
			public static readonly string Bootstrap5 = nameof(Bootstrap5);
			public static readonly string Bootstrap4 = nameof(Bootstrap4);
			public static readonly string Bootstrap3 = nameof(Bootstrap3);
		}
	}
}