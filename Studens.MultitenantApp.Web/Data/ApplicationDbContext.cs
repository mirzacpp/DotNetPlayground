using Microsoft.EntityFrameworkCore;
using Rev.AuthPermissions.AspNetCore.GetDataKeyCode;
using Rev.AuthPermissions.BaseCode.CommonCode;
using Rev.AuthPermissions.BaseCode.DataLayer.Classes;

namespace Studens.MultitenantApp.Web.Data
{
	/// <summary>
	/// Configures application dbcontext
	/// </summary>
	public class ApplicationDbContext : DbContext, IDataKeyFilterReadOnly
	{
		public string DataKey { get; }

		public ApplicationDbContext(
		DbContextOptions<ApplicationDbContext> options,
		IGetShardingDataFromUser dataKeyAccessor) : base(options)
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
		public DbSet<TeamMember> TeamMembers { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			//Apply base configuration
			base.OnModelCreating(builder);

			// Move this to extension method
			foreach (var entityType in builder.Model.GetEntityTypes())
			{
				//Make sure entity is not property-bag
				if (!entityType.IsPropertyBag && typeof(IDataKeyFilterReadWrite).IsAssignableFrom(entityType.ClrType))
				{
					entityType.AddSingleTenantShardingQueryFilter(this);
				}
				else
				{
					// TODO: Log this as a warning instead?
					//throw new Exception(
					//	$"You haven't added the {nameof(IDataKeyFilterReadWrite)} to the entity {entityType.ClrType.Name}");
				}

				foreach (var mutableProperty in entityType.GetProperties())
				{
					if (mutableProperty.ClrType == typeof(decimal))
					{
						mutableProperty.SetPrecision(9);
						mutableProperty.SetScale(2);
					}
				}
			}
		}

		public override int SaveChanges(bool acceptAllChangesOnSuccess)
		{
			this.MarkWithDataKeyIfNeeded(DataKey);
			return base.SaveChanges(acceptAllChangesOnSuccess);
		}

		public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
			CancellationToken cancellationToken = default)
		{
			this.MarkWithDataKeyIfNeeded(DataKey);
			return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
		}
	}
}