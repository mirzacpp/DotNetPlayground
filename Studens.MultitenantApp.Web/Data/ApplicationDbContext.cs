using Microsoft.EntityFrameworkCore;
using Rev.AuthPermissions.AspNetCore.GetDataKeyCode;
using Rev.AuthPermissions.BaseCode.CommonCode;
using Rev.AuthPermissions.BaseCode.DataLayer;
using Rev.AuthPermissions.BaseCode.DataLayer.EfCode;

namespace Studens.MultitenantApp.Web.Data
{
	public class ApplicationDbContext : AuthPermissionsDbContext,
	IDataKeyFilterReadOnly
	{
		public string DataKey { get; }

		public ApplicationDbContext(
		DbContextOptions<ApplicationDbContext> options,
		IGetShardingDataFromUser dataKeyAccessor,
		IRegisterStateChangeEvent? eventSetup = null)
			: base(options, eventSetup)
		{
			//Test this and handle case
			DataKey = dataKeyAccessor?.DataKey ?? "Data key is not set.";

			//NOTE: If no connection string is provided the DbContext will use the connection it was provided when it was registered.
			if (dataKeyAccessor?.ConnectionString != null)
			{
				Database.SetConnectionString(dataKeyAccessor.ConnectionString);
			}
		}

		public DbSet<Book> Books { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			//Apply base configuration
			base.OnModelCreating(builder);
		}
	}
}