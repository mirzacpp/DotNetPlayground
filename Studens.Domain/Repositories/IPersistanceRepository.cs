namespace Studens.Domain.Repositories
{
	/// <summary>
	/// Defines abstractions for entity persistance.
	/// </summary>
	public interface IPersistanceRepository<TEntity> where TEntity : class
	{
		Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

		Task InsertManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

		Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

		Task UpdateManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

		Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
	}
}