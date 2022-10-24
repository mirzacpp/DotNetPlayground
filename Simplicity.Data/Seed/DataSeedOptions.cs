namespace Simplicity.Data.Seed
{
	public class DataSeedOptions
	{
		/// <summary>
		/// Contains list of <see cref="IDataSeedContributor"/> types.
		/// </summary>
		public IList<Type> Contributors { get; set; } = new List<Type>();

		/// <summary>
		/// Contains current application environment.
		/// </summary>
		public string? Environment { get; set; }
	}
}