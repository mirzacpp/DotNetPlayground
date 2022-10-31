using Microsoft.Extensions.Configuration;

namespace Simplicity.Commons.Tests.Configuration
{
	public abstract class ConfigurationTestBase
	{
		protected IConfiguration Configuration { get; }

		public ConfigurationTestBase()
		{
			Configuration = new ConfigurationBuilder()
			.AddJsonFile("Configuration/appsettings.json")
			.Build();
		}
	}
}