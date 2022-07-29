namespace Studens.Data.Seed
{
	/// <summary>
	/// Defines data seed attribute for more control over <see cref="IDataSeedContributor"/> objects.
	/// Contributors not marked with attribute will not be ignored and will have order of <c>0</c>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class DataSeedAttribute : Attribute
	{
		/// <summary>
		/// Gets or sets order of the execution.
		/// </summary>
		public int Order { get; set; }

		/// <summary>
		/// Gets or sets flag indicating if contributor should be ignored on seed.
		/// </summary>
		public bool Ignore { get; set; }
	}
}