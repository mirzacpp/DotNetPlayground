using Microsoft.Extensions.DependencyInjection;

namespace Studens.AspNetCore.Mvc.UI.TagHelpers.Localization
{
	public static class LocalizationServiceCollectionExtensions
	{
		public static IServiceCollection AddDefaultLocalizationTagHelper(this IServiceCollection services)
		{
			services.AddSingleton<IInputControlGenerator, Bootstrap5ControlGenerator>();			


			return services;
		}
	}
}