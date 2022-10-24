using Microsoft.EntityFrameworkCore;
using Simplicity.Data.Migration;
using Simplicity.Data.Seed;

namespace Simplicity.MvcNet6.WebUI.Data
{
	public class DataMigrationManager : IDataMigrationManager
	{
		protected ApplicationDbContext DbContext { get; }
		protected IDataSeedManager SeedManager { get; }

		public DataMigrationManager(ApplicationDbContext dbContext, IDataSeedManager seedManager)
		{
			DbContext = dbContext;
			SeedManager = seedManager;
		}

		public async Task MigrateAsync()
		{
			await DbContext.Database.MigrateAsync();
			//Seed data
			await SeedManager.SeedAsync();
		}
	}
}