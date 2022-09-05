using AuthPermissions.AspNetCore.Services;
using Microsoft.EntityFrameworkCore;
using Rev.AuthPermissions.AspNetCore.GetDataKeyCode;
using Rev.AuthPermissions.BaseCode.DataLayer;
using Studens.Commons.Extensions;

namespace Studens.MultitenantApp.Web.Data
{
	public interface ITenantDbContextFactory
	{
		/// <summary>
		/// Constructs DbContext according to tenants <paramref name="databaseDataName"/> and <paramref name="dataKey"/>.
		/// </summary>
		/// <returns>DbContext</returns>
		ApplicationDbContext CreateTenantsDbContext(string databaseInfoName, string dataKey);
	}

	public class DbTenantContextFactory : ITenantDbContextFactory
	{
		private readonly IShardingConnections _shardingConnections;
		private readonly IRegisterStateChangeEvent? _eventSetup;
		private readonly DbContextOptions<ApplicationDbContext> _options;

		public DbTenantContextFactory(
		IShardingConnections shardingConnections,
		DbContextOptions<ApplicationDbContext> options,
		IRegisterStateChangeEvent? eventSetup = null)
		{
			_shardingConnections = shardingConnections;
			_eventSetup = eventSetup;
			_options = options;
		}

		public ApplicationDbContext CreateTenantsDbContext(string databaseInfoName, string dataKey)
		{
			string? connectionString = null;
			// Try to create connection string with tenants database name info
			// If connection string is not present, proceed to create context with current users tenant id.
			if (databaseInfoName.IsNotNullOrEmpty())
			{
				connectionString = _shardingConnections.FormConnectionString(databaseInfoName);
			}

			return new ApplicationDbContext(
			_options,
			   new StubGetShardingDataFromUser(connectionString, dataKey));
		}

		private class StubGetShardingDataFromUser : IGetShardingDataFromUser
		{
			public StubGetShardingDataFromUser(string connectionString, string dataKey)
			{
				ConnectionString = connectionString;
				DataKey = dataKey;
			}

			public string DataKey { get; }
			public string ConnectionString { get; }
		}
	}
}