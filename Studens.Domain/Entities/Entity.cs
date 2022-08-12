namespace Studens.Domain.Entities;

/// <inheritdoc cref="IEntity{TKey}"/>
public abstract class Entity<TKey> : IEntity<TKey>
{
	public Entity()
	{
	}

	public Entity(TKey id)
	{
		Id = id;
	}

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

	public static bool operator !=(Entity<TKey> a, Entity<TKey> b)
	{
		return !(a == b);
	}

	private bool IsTransient()
	{
		return Id is null || Id.Equals(default(TKey));
	}

	/// <inheritdoc/>
	public override string ToString() => $"[Entity: {GetType().Name}] Id = {Id}";
}

/// <inheritdoc/>
public abstract class Entity : Entity<long>
{
}