using Studens.Data.Migration;
using Studens.Data.Seed;

namespace Studens.MvcNet6.WebUI.Data
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
            await DbContext.Database.EnsureDeletedAsync();
            await DbContext.Database.EnsureCreatedAsync();

            await SeedManager.SeedAsync();
        }
    }
}