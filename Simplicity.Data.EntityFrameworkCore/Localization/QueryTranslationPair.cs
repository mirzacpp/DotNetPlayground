using Simplicity.Domain.Entities;

namespace Simplicity.Data.EntityFrameworkCore
{
	/// <summary>
	/// Represents pair of <typeparamref name="TTranslatableEntity"/> and <typeparamref name="TTranslation"/>
	/// </summary>
	/// <typeparam name="TTranslatableEntity">Translatable entity type</typeparam>
	/// <typeparam name="TTranslation">Entity translation type</typeparam>
	public class QueryTranslationPair<TTranslatableEntity, TTranslation> : QueryTranslationPair<TTranslatableEntity, int, TTranslation>
			where TTranslatableEntity : class, ITranslatableEntity<TTranslation>
			where TTranslation : class, IEntityTranslation<TTranslatableEntity, int>
	{
	}

	/// <summary>
	/// Represents pair of <typeparamref name="TTranslatableEntity"/> and <typeparamref name="TTranslation"/>
	/// </summary>
	/// <typeparam name="TTranslatableEntity">Translatable entity type</typeparam>
	/// <typeparam name="TTranslatableEntityKey">Translatable entity PK type</typeparam>
	/// <typeparam name="TTranslation">Entity translation type</typeparam>
	public class QueryTranslationPair<TTranslatableEntity, TTranslatableEntityKey, TTranslation>
		where TTranslatableEntity : class, ITranslatableEntity<TTranslation>
		where TTranslation : class, IEntityTranslation<TTranslatableEntity, TTranslatableEntityKey>
	{
		/// <summary>
		/// Entity being translated.
		/// </summary>
		public TTranslatableEntity Entity { get; set; }

		/// <summary>
		/// Retreived entity translation.
		/// </summary>
		public TTranslation Translation { get; set; }
	}
}