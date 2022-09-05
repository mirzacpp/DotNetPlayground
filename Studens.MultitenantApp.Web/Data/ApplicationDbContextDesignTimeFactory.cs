using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Studens.MultitenantApp.Web.Data
{
	public class ApplicationDbContextDesignTimeFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
	{
		public ApplicationDbContext CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
			var configuration = new ConfigurationBuilder()
			.AddCommandLine(args)
			.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
			.Build();

			optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

			return new ApplicationDbContext(optionsBuilder.Options, null);
		}
	}
}