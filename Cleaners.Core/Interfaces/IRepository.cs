namespace Cleaners.Core.Interfaces
{
    /// <summary>
    /// Interface for database access
    /// </summary>
    public interface IRepository
    {
        void Create<TEntity>(TEntity entity) where TEntity : class, IEntity, new();

        //int CreateAndGetId<TEntity>(TEntity entity) where TEntity : class, IEntity, new();

        void Update<TEntity>(TEntity entity) where TEntity : class, IEntity, new();

        void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity, new();

        void SoftDelete<TEntity>(TEntity entity) where TEntity : class, IEntity, ISoftDeletableEntity, new();

        void Restore<TEntity>(TEntity entity) where TEntity : class, IEntity, ISoftDeletableEntity, new();
    }
}