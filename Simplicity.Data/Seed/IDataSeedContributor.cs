namespace Simplicity.Data.Seed
{
    /// <summary>
    /// Contract for data seed contributor.
    /// </summary>
    public interface IDataSeedContributor
    {
        /// <summary>
        /// Executes seed logic.
        /// </summary>
        /// <returns>Task</returns>
        Task SeedDataAsync();
    }
}