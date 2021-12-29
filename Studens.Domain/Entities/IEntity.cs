namespace Studens.Domain.Entities
{
    /// <summary>
    /// Defines an entity with a single primary key with Id property
    /// </summary>
    /// <typeparam name="TKey">Type of the primary key for entity</typeparam>
    public interface IEntity<TKey>
    {
        public TKey Id { get; }
    }
}