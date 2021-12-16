using Studens.Domain.Domain;

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

    /// <inheritdoc/>
    public override string ToString() => $"[Entity: {GetType().Name}] Id = {Id}";
}
