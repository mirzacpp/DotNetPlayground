using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cleaners.Core.Interfaces
{
    /// <summary>
    /// Interface for database access
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Returns non-filtered collection for given DbSet
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="orderBy"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="trackEntities"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll<TEntity>(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? skip = null,
            int? take = null,
            bool trackEntities = false,
            params Expression<Func<TEntity, object>>[] includes) where TEntity : class, IEntity, new();

        /// <summary>
        /// Returns non-filtered collection for given DbSet
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="orderBy"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="trackEntities"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? skip = null,
            int? take = null,
            bool trackEntities = false,
            params Expression<Func<TEntity, object>>[] includes) where TEntity : class, IEntity, new();

        /// <summary>
        /// Returns filtered collection for given DbSet
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="orderByDesc"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="trackEntities"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        IEnumerable<TEntity> Get<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? skip = null,
            int? take = null,
            bool trackEntities = false,
            params Expression<Func<TEntity, object>>[] includes) where TEntity : class, IEntity, new();

        /// <summary>
        /// Returns filtered collection for given DbSet
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="orderByDesc"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="trackEntities"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAsync<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? skip = null,
            int? take = null,
            bool trackEntities = false,
            params Expression<Func<TEntity, object>>[] includes) where TEntity : class, IEntity, new();

        /// <summary>
        /// Returns first record that statisfieds filter
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="orderByDesc"></param>
        /// <param name="trackEntities"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        TEntity GetFirst<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            bool trackEntities = false,
            params Expression<Func<TEntity, object>>[] includes) where TEntity : class, IEntity, new();

        /// <summary>
        /// Returns first record that statisfieds filter
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="trackEntities"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<TEntity> GetFirstAsync<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            bool trackEntities = false,
            params Expression<Func<TEntity, object>>[] includes) where TEntity : class, IEntity, new();

        /// <summary>
        /// Returns record with given identifier
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity GetById<TEntity>(params object[] id) where TEntity : class, IEntity, new();

        /// <summary>
        /// Returns record with given identifier
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> GetByIdAsync<TEntity>(params object[] id) where TEntity : class, IEntity, new();

        /// <summary>
        /// Returns total number of records that match given filter
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        int GetCount<TEntity>(Expression<Func<TEntity, bool>> filter = null)
            where TEntity : class, IEntity, new();

        /// <summary>
        /// Returns total number of records that match given filter
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<int> GetCountAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null)
            where TEntity : class, IEntity, new();

        /// <summary>
        /// Returns total number of records that match given filter
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        long GetLongCount<TEntity>(Expression<Func<TEntity, bool>> filter = null)
            where TEntity : class, IEntity, new();

        /// <summary>
        /// Returns total number of records that match given filter
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<long> GetLongCountAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null)
            where TEntity : class, IEntity, new();

        /// <summary>
        /// Checks if any record matches given filter
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        bool GetExists<TEntity>(Expression<Func<TEntity, bool>> filter = null)
            where TEntity : class, IEntity, new();

        /// <summary>
        /// Checks if any record matches given filter
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<bool> GetExistsAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null)
            where TEntity : class, IEntity, new();

        /// <summary>
        /// Marks entity for create
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        void Create<TEntity>(TEntity entity) where TEntity : class, IEntity, new();

        /// <summary>
        /// Marks entity for update
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        void Update<TEntity>(TEntity entity) where TEntity : class, IEntity, new();

        /// <summary>
        /// Marks entity for delete
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity, new();

        /// <summary>
        /// Marks entity for soft delete
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        void SoftDelete<TEntity>(TEntity entity) where TEntity : class, IEntity, ISoftDeletableEntity, new();

        /// <summary>
        /// Marks entity for restore
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        void Restore<TEntity>(TEntity entity) where TEntity : class, IEntity, ISoftDeletableEntity, new();

        /// <summary>
        /// Saves all changes to database
        /// </summary>
        /// <returns></returns>
        int SaveChanges();

        /// <summary>
        /// Saves all changes to database
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();
    }
}