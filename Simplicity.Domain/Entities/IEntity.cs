namespace Simplicity.Domain.Entities
{
	/// <summary>
	/// Defines an entity contract with single or composite primary key.
	/// </summary>
	public interface IEntity
	{
		/// <summary>
		/// Returns an array of ordered keys for this entity.
		/// </summary>
		object[] GetKeys();
	}

	/// <summary>
	/// Defines an entity with a single primary key with Id property.	
	/// </summary>
	/// <typeparam name="TKey">Type of the primary key for entity</typeparam>
	public interface IEntity<TKey> : IEntity
	{
		/// <summary>
		/// Unique identifier for this entity.
		/// </summary>
		public TKey Id { get; }
	}
}