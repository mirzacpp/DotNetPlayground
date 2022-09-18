using Microsoft.AspNetCore.Identity;
using Studens.Data.Seed;

namespace Studens.MvcNet6.WebUI.Data
{
    public class RoleDataSeedContributor : IDataSeedContributor
    {
        protected ApplicationDbContext DbContext { get; }

		public int Order { get; } = 10;

		public RoleDataSeedContributor(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task SeedDataAsync()
        {
            var dbSet = DbContext.Set<IdentityRole>();

            dbSet.Add(new IdentityRole("TestRole1"));
            dbSet.Add(new IdentityRole("TestRole2"));

            await DbContext.SaveChangesAsync();
        }
    }
}