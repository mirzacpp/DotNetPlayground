using Microsoft.AspNetCore.Identity;
using Studens.Data.Extensions;
using Studens.Data.Seed;
using System.Text.Json;

namespace Studens.MvcNet6.WebUI.Data
{
	[DataSeed(Ignore = true)]
    public class Role3DataSeedContributor : IDataSeedContributor
    {
        protected ApplicationDbContext DbContext { get; }

		public int Order { get; }

		public Role3DataSeedContributor(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task SeedDataAsync()
        {
            var dbSet = DbContext.Set<IdentityRole>();
            
            var roles = JsonHelper.DeserializeFromFile<List<IdentityRole>>(Path.Combine(Directory.GetCurrentDirectory(), "Data/roles.json"));            
            dbSet.AddRange(roles);

            await DbContext.SaveChangesAsync();
        }
    }
}