using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Studens.Data.Seed
{
    /// <summary>
    /// Default data seed manager implementation.
    /// </summary>
    public class DataSeedManager : IDataSeedManager
    {
        public IServiceScopeFactory ServiceScopeFactory { get; }
        public DataSeedOptions Options { get; }

        public DataSeedManager(IServiceScopeFactory serviceScopeFactory, IOptions<DataSeedOptions> optionAccessor)
        {
            ServiceScopeFactory = serviceScopeFactory;
            Options = optionAccessor.Value;
        }

        public async Task SeedAsync()
        {
            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var test = scope.ServiceProvider.GetRequiredService<IDataSeedManager>();                

                // grab contributor and seed data
                foreach (var contributorType in Options.Contributors)
                {
                    var contributor = (IDataSeedContributor)scope
                        .ServiceProvider
                        .GetRequiredService(contributorType);

                    await contributor.SeedDataAsync();
                }
            }
        }
    }
}