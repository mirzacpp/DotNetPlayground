namespace Studens.Data.Seed
{
    /// <summary>
    /// Contract for data seed contributor.
    /// </summary>
    public interface IDataSeedContributor
    {
        Task SeedDataAsync();
    }
}