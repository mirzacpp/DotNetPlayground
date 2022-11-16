namespace Simplicity.Commons.Tests.Timing
{
	public class DateTimeProviderTests
	{
		private IDateTimeProvider DateTimeProvider { get; }

		public DateTimeProviderTests()
		{
			DateTimeProvider = new DateTimeProvider();
		}

		[Theory]
		[InlineData("Europe/Mostar")]
		public void Can_detect_invalid_time_zone(string timeZoneId)
		{
			var result = DateTimeProvider.TimeZoneExists(timeZoneId);

			result.ShouldBe(false);
		}
	}
}