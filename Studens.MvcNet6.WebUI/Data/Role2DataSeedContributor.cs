using Microsoft.AspNetCore.Identity;
using Studens.Data.Seed;

namespace Studens.MvcNet6.WebUI.Data
{
    public class Role2DataSeedContributor : IDataSeedContributor
    {
        protected ApplicationDbContext DbContext { get; }

        public Role2DataSeedContributor(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task SeedDataAsync()
        {
            var dbSet = DbContext.Set<IdentityRole>();

            dbSet.Add(new IdentityRole("TestRole3"));
            dbSet.Add(new IdentityRole("TestRole4"));

            await DbContext.SaveChangesAsync();
        }
    }
}