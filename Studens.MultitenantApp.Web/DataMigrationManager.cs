using Microsoft.EntityFrameworkCore;
using Rev.AuthPermissions.BaseCode.DataLayer.EfCode;
using Studens.Data.Migration;
using Studens.Data.Seed;
using Studens.MultitenantApp.Web.Data;

namespace Studens.MultitenantApp.Web
{
	public class DataMigrationManager : IDataMigrationManager
	{
		protected AuthPermissionsDbContext DbContext { get; }
		protected IDataSeedManager SeedManager { get; }

		public DataMigrationManager(AuthPermissionsDbContext dbContext, IDataSeedManager seedManager)
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