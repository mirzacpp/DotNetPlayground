using Microsoft.EntityFrameworkCore;
using Studens.Data.Migration;
using Studens.Data.Seed;
using Studens.MultitenantApp.Web.Data;

namespace Studens.MultitenantApp.Web
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