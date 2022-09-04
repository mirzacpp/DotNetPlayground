using RunMethodsSequentially;
using Studens.Data.Migration;

namespace Studens.MultitenantApp.Web
{
	public class StartupServiceMigrateDatabase : IStartupServiceToRunSequentially
	{
		/// <summary>
		/// Set to -10 so that it is run before any other startup services
		/// </summary>
		public int OrderNum { get; } = -10; //These must be run before any other startup services

		public async ValueTask ApplyYourChangeAsync(IServiceProvider scopedServices)
		{
			var migrationManager = scopedServices.GetRequiredService<IDataMigrationManager>();

			await migrationManager.MigrateAsync();
		}
	}
}