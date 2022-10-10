using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Studens.MvcNet6.WebUI.OutOfProcess;

namespace Studens.MvcNet6.WebUI.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public DbSet<EmailQueue> EmailQueue { get; set; }

		public ApplicationDbContext(DbContextOptions options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);
		}
	}
}