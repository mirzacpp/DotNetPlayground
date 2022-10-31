using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Simplicity.Commons.Tests.Configuration
{
	public class ServiceCollectionExtensionsTests : ConfigurationTestBase
	{
		[Fact]
		public void Add_poco_options_to_service_collection()
		{
			var services = new ServiceCollection();			

			services.AddPocoOptions<TestOptions>(nameof(TestOptions), Configuration);
			var serviceProvider = services.BuildServiceProvider();
			var options = serviceProvider.GetRequiredService<TestOptions>();

			options.Int.ShouldBe(2);
			options.String.ShouldBe("test-text");
			options.Bool.ShouldBe(true);
			options.Date.ShouldBe(new DateTime(2022, 10, 19, 15, 30, 10, DateTimeKind.Utc));
		}

		[Fact]
		public void Add_poco_options_to_service_collection_with_out_parameter()
		{
			var services = new ServiceCollection();

			services.AddPocoOptions<TestOptions>(nameof(TestOptions), Configuration, out var options);
			var serviceProvider = services.BuildServiceProvider();
			var testOptions = serviceProvider.GetRequiredService<TestOptions>();

			testOptions.ShouldBeSameAs(options);
		}
	}
}