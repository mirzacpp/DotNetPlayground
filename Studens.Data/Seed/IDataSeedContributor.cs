namespace Studens.Data.Seed
{
	/// <summary>
	/// Contract for data seed contributor.
	/// </summary>
	public interface IDataSeedContributor
	{
		/// <summary>
		/// Represents an order in which contribution should be executed.
		/// Lower values are executed first.
		/// </summary>
		/// <remarks>
		/// Defaults to 0.
		/// </remarks>
		int Order { get; }

		/// <summary>
		/// Executes seed logic.
		/// </summary>
		/// <returns>Task</returns>
		Task SeedDataAsync();
	}
}