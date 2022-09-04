using Microsoft.EntityFrameworkCore;
using Studens.MultitenantApp.Web.Data;

namespace AuthPermissions.AspNetCore.Services;

/// <summary>
/// Introduced so we do not tangle <see cref="IShardingConnections"/> with database calls.
/// This will also cause circular dependencies through <see cref="Rev.AuthPermissions.AspNetCore.GetDataKeyCode.IGetShardingDataFromUser"/>.
/// This service will server as a mediator between these two services.
/// </summary>
public interface ITenantShardingService
{
	/// <summary>
	/// This returns all the database info names in the shardingsetting.json file, with a list of tenant name linked to each connection name
	/// </summary>
	/// <returns>List of all the database info names with the tenants (and whether its sharding) within that database data name
	/// NOTE: The hasOwnDb is true for a database containing a single database, false for multiple tenant database and null if empty</returns>
	Task<List<(string databaseInfoName, bool? hasOwnDb, List<string> tenantNames)>> GetDatabaseInfoNamesWithTenantNamesAsync();
}

public class TenantShardingService : ITenantShardingService
{
	private readonly IShardingConnections _shardingConnections;
	private readonly ApplicationDbContext _context;

	public TenantShardingService(IShardingConnections shardingConnections, ApplicationDbContext context)
	{
		_shardingConnections = shardingConnections;
		_context = context;
	}

	/// <summary>
	/// This returns all the database info names in the shardingsetting.json file, with a list of tenant name linked to each connection name
	/// NOTE: The DatabaseInfoName which matches the <see cref="AuthPermissionsOptions.ShardingDefaultDatabaseInfoName"/> is always
	/// returns a HasOwnDb value of false. This is because the default database has the AuthP data in it.
	/// </summary>
	/// <returns>List of all the database info names with the tenants using that database data name
	/// NOTE: The hasOwnDb is true for a database containing a single database, false for multiple tenant database and null if empty</returns>
	public async Task<List<(string databaseInfoName, bool? hasOwnDb, List<string> tenantNames)>> GetDatabaseInfoNamesWithTenantNamesAsync()
	{
		var nameAndConnectionName = await _context.Tenants
			.Select(x => new { ConnectionName = x.DatabaseInfoName, x })
			.ToListAsync();

		var grouped = nameAndConnectionName.GroupBy(x => x.ConnectionName)
			.ToDictionary(x => x.Key,
				y => y.Select(z => new { z.x.HasOwnDb, z.x.TenantName }));


		var shardingDefaultDatabaseInfoName = _shardingConnections.GetShardingDefaultDatabaseInfoName();
		var result = new List<(string databaseInfoName, bool? hasOwnDb, List<string>)>();
		//Add sharding database names that have no tenants in them so that you can see all the connection string  names
		foreach (var databaseInfoName in _shardingConnections.GetAllPossibleShardingData().Select(x => x.Name))
		{
			result.Add(grouped.ContainsKey(databaseInfoName)
				? (databaseInfoName,
					databaseInfoName == shardingDefaultDatabaseInfoName
						? false //The default DatabaseInfoName contains the AuthP information, so its a shared database
						: grouped[databaseInfoName].FirstOrDefault()?.HasOwnDb,
					grouped[databaseInfoName].Select(x => x.TenantName).ToList())
				: (databaseInfoName,
					databaseInfoName == shardingDefaultDatabaseInfoName ? false : null,
					new List<string>()));
		}

		return result;
	}
}