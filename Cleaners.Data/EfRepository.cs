using Cleaners.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace Cleaners.Data
{
    /// <summary>
    /// Repository implementation for EntityFramework
    /// </summary>
    public class EfRepository : IRepository
    {
        private readonly DbContext _dbContext;

        public EfRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (entity is IAuditableEntity auditableEntity)
            {
                auditableEntity.CreationDateUtc = DateTime.UtcNow;
            }

            _dbContext.Set<TEntity>().Add(entity);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbContext.Set<TEntity>().Remove(entity);
        }

        public void Restore<TEntity>(TEntity entity) where TEntity : class, IEntity, ISoftDeletableEntity, new()
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            (entity as ISoftDeletableEntity).IsDeleted = false;

            Update(entity);
        }

        public void SoftDelete<TEntity>(TEntity entity) where TEntity : class, IEntity, ISoftDeletableEntity, new()
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            (entity as ISoftDeletableEntity).IsDeleted = true;

            Update(entity);
        }

        public void Update<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (entity is IAuditableEntity auditableEntity)
            {
                auditableEntity.LastUpdateDateUtc = DateTime.UtcNow;
            }

            _dbContext.Set<TEntity>().Update(entity);
        }
    }
}