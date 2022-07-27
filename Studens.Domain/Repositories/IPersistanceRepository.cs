namespace Studens.Domain.Repositories
{
	/// <summary>
	/// Defines abstractions for entity persistance.
	/// </summary>
	public interface IPersistanceRepository<TEntity> where TEntity : class
	{
		Task<TEntity> InsertAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default);

		Task InsertManyAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

		Task<TEntity> UpdateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default);

		Task UpdateManyAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

		Task DeleteAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default);
	}
}