namespace Simplicity.Domain.Entities
{
	/// <summary>
	/// Defines an entity with a single primary key with Id property
	/// TODO: Derive this interface from non generic one so we can use it for a constraint.
	/// </summary>
	/// <typeparam name="TKey">Type of the primary key for entity</typeparam>
	public interface IEntity<TKey>
	{
		public TKey Id { get; }
	}
}