using Microsoft.Extensions.Logging;

namespace Simplicity.Data.Seed
{
	/// <summary>
	/// Logging extension methods used for seed process logging.
	/// </summary>
	public static partial class LoggingExtensions
	{
		[LoggerMessage(
				EventId = 200,
				Level = LogLevel.Information,
				Message = "Started seeding data for contributor {contributorType}.")]
		public static partial void DataSeedStarted(this ILogger logger, Type contributorType);

		[LoggerMessage(
				EventId = 201,
				Level = LogLevel.Information,
				Message = "Finished seeding data for contributor {contributorType}.")]
		public static partial void DataSeedFinished(this ILogger logger, Type contributorType);

		[LoggerMessage(
				EventId = 202,
				Level = LogLevel.Error,
				Message = "Error occured while trying to seed data for contributor {contributorType}")]
		public static partial void DataSeedError(this ILogger logger, Type contributorType, Exception ex);
	}
}