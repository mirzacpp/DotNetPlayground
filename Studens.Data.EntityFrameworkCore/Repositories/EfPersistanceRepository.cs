using Microsoft.EntityFrameworkCore;
using Studens.Domain.Repositories;

namespace Studens.Data.EntityFrameworkCore.Repositories
{
	public class EfPersistanceRepository<TDbContext, TEntity> : IPersistanceRepository<TEntity>
	where TDbContext : DbContext
	where TEntity : class
	{
		private readonly TDbContext _dbContext;

		public EfPersistanceRepository(TDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task DeleteAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
		{
			await _dbContext.SaveChangesAsync();
		}

		public Task<TEntity> InsertAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task InsertManyAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<TEntity> UpdateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task UpdateManyAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}
	}
}