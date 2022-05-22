namespace Studens.Data.Seed
{
    public class DataSeedOptions
    {
        /// <summary>
        /// Contains list of <see cref="IDataSeedContributor"/> types.
        /// </summary>
        public IList<Type> Contributors { get; set; } = new List<Type>();
    }
}