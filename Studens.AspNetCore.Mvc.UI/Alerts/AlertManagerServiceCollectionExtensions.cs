using Microsoft.Extensions.DependencyInjection;

namespace Studens.AspNetCore.Mvc.UI.Alerts
{
	/// <summary>
	/// Extension methods for alert manager registration on startup
	/// </summary>
	public static class AlertManagerServiceCollectionExtensions
	{
		/// <summary>
		/// Registers <see cref="TempDataAlertManager"/> implementation of <see cref="IAlertManager"/>
		/// </summary>
		/// <param name="services">Service collection</param>
		public static void AddTempDataAlertManager(this IServiceCollection services)
		{
			if (services == null)
			{
				throw new ArgumentNullException(nameof(services));
			}

			services.AddScoped<IAlertManager, TempDataAlertManager>();
		}
	}
}