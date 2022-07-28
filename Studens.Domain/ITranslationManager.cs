using Studens.Domain.Entities;

namespace Studens.Domain
{
	/// <summary>
	/// Abstraction for translated entities processor.
	/// </summary>
	public interface ITranslationManager
	{
		/// <summary>
		/// Returns missing translations either by parent culture or by default culture.
		/// </summary>
		/// <typeparam name="TTranslatableEntity"></typeparam>
		/// <typeparam name="TTranslatableEntityKey"></typeparam>
		/// <typeparam name="TTranslation"></typeparam>
		/// <param name="entityIds"></param>
		/// <param name="culture"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<IList<TTranslation>> GetMissingTranslationsAsync<TTranslatableEntity, TTranslatableEntityKey, TTranslation>(
		IList<TTranslatableEntityKey> entityIds,
		CancellationToken cancellationToken = default)
		where TTranslation : class, IEntityTranslation<TTranslatableEntity, TTranslatableEntityKey>
		where TTranslatableEntity : class, ITranslatableEntity<TTranslation>;			
	}
}