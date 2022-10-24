using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Simplicity.Data.Seed
{
	/// <summary>
	/// Default implementation for <see cref="IDataSeedManager"/>.
	/// </summary>
	public class DataSeedManager : IDataSeedManager
	{
		public IServiceScopeFactory ServiceScopeFactory { get; }
		public DataSeedOptions Options { get; }
		public ILogger<DataSeedManager> Logger { get; }

		public DataSeedManager(IServiceScopeFactory serviceScopeFactory,
		IOptions<DataSeedOptions> optionAccessor,
		ILogger<DataSeedManager> logger)
		{
			ServiceScopeFactory = serviceScopeFactory;
			Options = optionAccessor.Value;
			Logger = logger;
		}

		/// <summary>
		/// Executes all registered data seed contributors.
		/// </summary>
		/// <returns>Task</returns>
		public async Task SeedAsync()
		{
			using var scope = ServiceScopeFactory.CreateScope();
			// Filter out contributors and seed data
			var contributors = Options.Contributors
			.Active()
			.WithEnvironment(Options.Environment)
			.Ordered();

			foreach (var contributorType in contributors)
			{
				try
				{
					var contributor = (IDataSeedContributor)scope
					.ServiceProvider
					.GetRequiredService(contributorType);

					Logger.DataSeedStarted(contributorType);
					await contributor.SeedDataAsync();
					Logger.DataSeedFinished(contributorType);
				}
				catch (Exception ex)
				{
					Logger.DataSeedError(contributorType, ex);
					// Rethrow exception
					throw;
				}
			}
		}
	}
}