using Cleaners.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cleaners.Data
{
    /// <summary>
    /// Repository implementation for EntityFramework
    /// </summary>
    public class EfRepository : IRepository
    {
        #region Fields

        private readonly DbContext _context;

        #endregion Fields

        #region Constructor

        public EfRepository(DbContext context)
        {
            _context = context;
        }

        #endregion Constructor

        #region Create/Update/Delete

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

            _context.Set<TEntity>().Add(entity);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _context.Set<TEntity>().Remove(entity);
        }

        public void Restore<TEntity>(TEntity entity) where TEntity : class, IEntity, ISoftDeletableEntity, new()
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            (entity as ISoftDeletableEntity).IsSoftDeleted = false;

            Update(entity);
        }

        public void SoftDelete<TEntity>(TEntity entity) where TEntity : class, IEntity, ISoftDeletableEntity, new()
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            (entity as ISoftDeletableEntity).IsSoftDeleted = true;

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

            _context.Set<TEntity>().Update(entity);
        }

        #endregion Create/Update/Delete

        #region Read

        /// <summary>
        /// Central method used to create queries
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="trackEntities"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> GetQuery<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? skip = null,
            int? take = null,
            bool trackEntities = false,
            params Expression<Func<TEntity, object>>[] includes) where TEntity : class, IEntity, new()
        {
            var query = trackEntities ?
                _context.Set<TEntity>() :
                _context.Set<TEntity>().AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includes != null)
            {
                query = includes.Aggregate(query, (entity, include) => entity.Include(include));
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip != null)
            {
                query = query.Skip(skip.Value);
            }

            if (take != null)
            {
                query = query.Take(take.Value);
            }

            return query;
        }

        public IEnumerable<TEntity> GetAll<TEntity>(
            Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy,
            int? skip,
            int? take,
            bool trackEntities,
            params Expression<Func<TEntity, object>>[] includes) where TEntity : class, IEntity, new()
        {
            return GetQuery(null, orderBy, skip, take, trackEntities, includes).ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(
            Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy,
            int? skip,
            int? take,
            bool trackEntities,
            params Expression<Func<TEntity, object>>[] includes) where TEntity : class, IEntity, new()
        {
            return await GetQuery(null, orderBy, skip, take, trackEntities, includes).ToListAsync();
        }

        public IEnumerable<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? skip = null,
            int? take = null,
            bool trackEntities = false,
            params Expression<Func<TEntity, object>>[] includes) where TEntity : class, IEntity, new()
        {
            return GetQuery(filter, orderBy, skip, take, trackEntities, includes).ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAsync<TEntity>(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            int? skip,
            int? take,
            bool trackEntities,
            params Expression<Func<TEntity, object>>[] includes) where TEntity : class, IEntity, new()
        {
            return await GetQuery(filter, orderBy, skip, take, trackEntities, includes).ToListAsync();
        }

        public TEntity GetById<TEntity>(params object[] id) where TEntity : class, IEntity, new()
        {
            if (id == null)
            {
                return null;
            }

            return _context.Set<TEntity>().Find(id);
        }

        public async Task<TEntity> GetByIdAsync<TEntity>(params object[] id) where TEntity : class, IEntity, new()
        {
            if (id == null)
            {
                return null;
            }

            return await _context.Set<TEntity>().FindAsync(id);
        }

        public int GetCount<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class, IEntity, new()
        {
            return GetQuery(filter).Count();
        }

        public async Task<int> GetCountAsync<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class, IEntity, new()
        {
            return await GetQuery(filter).CountAsync();
        }

        public bool GetExists<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class, IEntity, new()
        {
            return GetQuery(filter).Any();
        }

        public async Task<bool> GetExistsAsync<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class, IEntity, new()
        {
            return await GetQuery(filter).AnyAsync();
        }

        public TEntity GetFirst<TEntity>(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            bool trackEntities,
            params Expression<Func<TEntity, object>>[] includes) where TEntity : class, IEntity, new()
        {
            return GetQuery(filter, orderBy, null, null, trackEntities, includes).FirstOrDefault();
        }

        public async Task<TEntity> GetFirstAsync<TEntity>(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            bool trackEntities,
            params Expression<Func<TEntity, object>>[] includes) where TEntity : class, IEntity, new()
        {
            return await GetQuery(filter, orderBy, null, null, trackEntities, includes).FirstOrDefaultAsync();
        }

        public long GetLongCount<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class, IEntity, new()
        {
            return GetQuery(filter).LongCount();
        }

        public async Task<long> GetLongCountAsync<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class, IEntity, new()
        {
            return await GetQuery(filter).LongCountAsync();
        }

        #endregion Read

        #region Save

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        #endregion Save
    }
}