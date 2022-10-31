namespace Simplicity.Domain.Entities;

/// <inheritdoc cref="IEntity{TKey}"/>
public abstract class Entity<TKey> : IEntity<TKey>
{
	protected Entity()
	{
	}

	protected Entity(TKey id)
	{
		Id = id;
	}

	public object[] GetKeys() => new object[] { Id };

	public virtual TKey Id { get; protected set; }

	public override bool Equals(object? obj)
	{
		if (obj is not Entity<TKey> other)
		{
			return false;
		}

		if (ReferenceEquals(this, other))
		{
			return true;
		}

		if (IsTransient() || other.IsTransient())
		{
			return false;
		}

		return Id.Equals(other.Id);
	}

	public static bool operator ==(Entity<TKey> a, Entity<TKey> b)
	{
		if (a is null && b is null)
			return true;

		if (a is null || b is null)
			return false;

		return a.Equals(b);
	}

	public static bool operator !=(Entity<TKey> a, Entity<TKey> b) => !(a == b);

	private bool IsTransient() => Id is null || Id.Equals(default(TKey));

	/// <inheritdoc/>
	public override string ToString() => $"[Entity: {GetType().Name}] Id = {Id}";

	public override int GetHashCode()
	{
		throw new NotImplementedException();
	}
}

/// <inheritdoc/>
public abstract class Entity : Entity<long>
{

}