﻿using Microsoft.AspNetCore.Identity;
using Simplicity.Data.Seed;

namespace Simplicity.MvcNet6.WebUI.Data
{
	public class Role2DataSeedContributor : IDataSeedContributor
	{
		protected ApplicationDbContext DbContext { get; }
		public int Order { get; }

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