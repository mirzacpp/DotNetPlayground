using Microsoft.Extensions.Configuration;

namespace Simplicity.Commons.Tests.Configuration
{
	public class ConfigurationExtensionsTests : ConfigurationTestBase
	{
		[Fact]
		public void Bind_integer_correctly()
		{
			var value = Configuration.GetRequiredValue<int>(nameof(TestOptions.Int));

			value.ShouldBe(2);
		}

		[Fact]
		public void Throw_exception_for_non_existing_integer()
		{
			var exception = Should.Throw<InvalidOperationException>(() => Configuration.GetRequiredValue<int>("NonExistingInt"));

			exception.Message.ShouldBe("'NonExistingInt' had no value, make sure it has been added to the Configuration.");
		}

		[Fact]
		public void Bind_string_correctly()
		{
			var value = Configuration.GetRequiredValue<string>(nameof(TestOptions.String));

			value.ShouldBe("test-text");
		}

		[Fact]
		public void Throw_exception_for_non_existing_string()
		{
			var exception = Should.Throw<InvalidOperationException>(() => Configuration.GetRequiredValue<int>("NonExistingString"));

			exception.Message.ShouldBe("'NonExistingString' had no value, make sure it has been added to the Configuration.");
		}

		[Fact]
		public void Bind_boolean_correctly()
		{
			var value = Configuration.GetRequiredValue<bool>(nameof(TestOptions.Bool));

			value.ShouldBe(true);
		}

		[Fact]
		public void Throw_exception_for_non_existing_boolean()
		{
			var exception = Should.Throw<InvalidOperationException>(() => Configuration.GetRequiredValue<bool>("NonExistingBool"));

			exception.Message.ShouldBe("'NonExistingBool' had no value, make sure it has been added to the Configuration.");
		}

		[Fact]
		public void Bind_date_time_offset_correctly()
		{
			var value = Configuration.GetRequiredValue<DateTimeOffset>(nameof(TestOptions.Date));

			value.ShouldBe(new DateTime(2022, 10, 19, 15, 30, 10, DateTimeKind.Utc));
		}

		[Fact]
		public void Throw_exception_for_non_existing_date_time_offset()
		{
			var exception = Should.Throw<InvalidOperationException>(() => Configuration.GetRequiredValue<DateTimeOffset>("NonExistingDate"));

			exception.Message.ShouldBe("'NonExistingDate' had no value, make sure it has been added to the Configuration.");
		}

		[Fact]
		public void Bind_complex_array_correctly()
		{
			var values = Configuration.GetRequiredValues<TestOptions>("Collection");

			values.Count().ShouldBe(3);
			values.ShouldContain(v => v.Int == 1 && v.String == "test-text-1" && v.Bool);
			values.ShouldContain(v => v.Int == 2 && v.String == "test-text-2" && v.Bool);
			values.ShouldContain(v => v.Int == 3 && v.String == "test-text-3" && v.Bool);
		}

		[Fact]
		public void Bind_date_times_in_complex_array_correctly()
		{
			var values = Configuration.GetRequiredValues<TestOptions>("Collection");

			var firstItem = values.First();

			values.Count().ShouldBe(3);
			firstItem.Date.ShouldBe(new DateTime(2022, 10, 19, 15, 30, 10, DateTimeKind.Utc));
		}

		[Fact]
		public void Throw_exception_when_trying_to_bind_enumerable_from_non_existing_section()
		{
			var exception = Should.Throw<InvalidOperationException>(() => Configuration.GetRequiredValues<TestOptions>("nonExistingCollection"));

			exception.Message.ShouldBe("Section 'nonExistingCollection' not found in configuration.");
		}

		[Fact]
		public void Throw_exception_when_trying_to_bind_to_non_existing_section()
		{
			var exception = Should.Throw<Exception>(() => Configuration.GetRequiredOptions<TestOptions>("nonExistingTestOptions"));

			exception.Message.ShouldBe("Section 'nonExistingTestOptions' not found in configuration.");
		}
	}
}