using Microsoft.EntityFrameworkCore;
using Simplicity.Domain.Repositories;

namespace Simplicity.Data.EntityFrameworkCore.Repositories
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

		public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
		{
			await _dbContext.SaveChangesAsync();
		}

		public Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task InsertManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task UpdateManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}
	}
}