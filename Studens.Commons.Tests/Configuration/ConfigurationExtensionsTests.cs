using Microsoft.Extensions.Configuration;
using Shouldly;

namespace Studens.Commons.Tests.Configuration
{
	public class ConfigurationExtensionsTests
	{
		private IConfiguration Configuration { get; }

		public ConfigurationExtensionsTests()
		{
			Configuration = new ConfigurationBuilder()
			.AddJsonFile("Configuration/appsettings.json")
			.Build();
		}

		[Fact]
		public void ShouldBindIntegerCorrectly()
		{
			var value = Configuration.GetRequiredValue<int>(nameof(TestOptions.Int));

			value.ShouldBe(2);
		}

		[Fact]
		public void ShouldThrowExceptionForNonExistingInteger()
		{
			var exception = Should.Throw<InvalidOperationException>(() => Configuration.GetRequiredValue<int>("NonExistingInt"));

			exception.Message.ShouldBe("'NonExistingInt' had no value, make sure it has been added to the Configuration.");
		}

		[Fact]
		public void ShouldBindStringCorrectly()
		{
			var value = Configuration.GetRequiredValue<string>(nameof(TestOptions.String));

			value.ShouldBe("test-text");
		}

		[Fact]
		public void ShouldThrowExceptionForNonExistingString()
		{
			var exception = Should.Throw<InvalidOperationException>(() => Configuration.GetRequiredValue<int>("NonExistingString"));

			exception.Message.ShouldBe("'NonExistingString' had no value, make sure it has been added to the Configuration.");
		}

		[Fact]
		public void ShouldBindBooleanCorrectly()
		{
			var value = Configuration.GetRequiredValue<bool>(nameof(TestOptions.Bool));

			value.ShouldBe(true);
		}

		[Fact]
		public void ShouldThrowExceptionForNonExistingBoolean()
		{
			var exception = Should.Throw<InvalidOperationException>(() => Configuration.GetRequiredValue<bool>("NonExistingBool"));

			exception.Message.ShouldBe("'NonExistingBool' had no value, make sure it has been added to the Configuration.");
		}

		[Fact]
		public void ShouldBindDateTimeOffsetCorrectly()
		{
			var value = Configuration.GetRequiredValue<DateTimeOffset>(nameof(TestOptions.Date));

			value.ShouldBe(new DateTime(2022, 10, 19, 15, 30, 10, DateTimeKind.Utc));
		}

		[Fact]
		public void ShouldThrowExceptionForNonExistingDateTimeOffset()
		{
			var exception = Should.Throw<InvalidOperationException>(() => Configuration.GetRequiredValue<DateTimeOffset>("NonExistingDate"));

			exception.Message.ShouldBe("'NonExistingDate' had no value, make sure it has been added to the Configuration.");
		}

		[Fact]
		public void ShouldBindComplexArrayCorrectly()
		{
			var values = Configuration.GetRequiredValues<TestOptions>("Collection");

			values.Count().ShouldBe(3);
			values.ShouldContain(v => v.Int == 1 && v.String == "test-text-1" && v.Bool);
			values.ShouldContain(v => v.Int == 2 && v.String == "test-text-2" && v.Bool);
			values.ShouldContain(v => v.Int == 3 && v.String == "test-text-3" && v.Bool);
		}

		[Fact]
		public void ShouldBindDateTimesInComplexArrayCorrectly()
		{
			var values = Configuration.GetRequiredValues<TestOptions>("Collection");

			var firstItem = values.First();

			values.Count().ShouldBe(3);
			firstItem.Date.ShouldBe(new DateTime(2022, 10, 19, 15, 30, 10, DateTimeKind.Utc));
		}

		[Fact]
		public void ShouldThrowExceptionWhenTryingToBindEnumerableFromNonExistingSection()
		{
			var exception = Should.Throw<InvalidOperationException>(() => Configuration.GetRequiredValues<TestOptions>("nonExistingCollection"));

			exception.Message.ShouldBe("Section 'nonExistingCollection' not found in configuration.");
		}

		[Fact]
		public void ShouldThrowExceptionWhenTryingToBindToNonExistingSection()
		{
			var exception = Should.Throw<Exception>(() => Configuration.GetRequiredOptions<TestOptions>("nonExistingTestOptions"));

			exception.Message.ShouldBe("Section 'nonExistingTestOptions' not found in configuration.");
		}
	}
}