using Studens.Data.Localization;

namespace Studens.Localization.EntityFrameworkCore
{
	public class QueryTranslationPair<TTranslatableEntity, TTranslation>
			where TTranslatableEntity : class, ITranslatableEntity<TTranslation>
			where TTranslation : class, IEntityTranslation<TTranslatableEntity, int>
	{
		public QueryTranslationPair()
		{

		}

		public QueryTranslationPair(TTranslatableEntity entity, TTranslation translation)
		{
			Entity = entity;
			Translation = translation;
		}

		/// <summary>
		/// Entity being translated
		/// </summary>
		public TTranslatableEntity Entity { get; set; }

		/// <summary>
		/// Retreived entity translation
		/// </summary>
		public TTranslation Translation { get; set; }
	}

	//public sealed class QueryTranslationPair<TTranslatableEntity, TTranslatableEntityKey, TTranslation>
	//	where TTranslatableEntity : class, ITranslatableEntity<TTranslation>
	//	where TTranslation : class, IEntityTranslation<TTranslatableEntity, TTranslatableEntityKey>
	//{
	//	public QueryTranslationPair()
	//	{

	//	}

	//	public QueryTranslationPair(TTranslatableEntity entity, TTranslation translation)
	//	{
	//		Entity = entity;
	//		Translation = translation;
	//	}

	//	/// <summary>
	//	/// Entity being translated
	//	/// </summary>
	//	public TTranslatableEntity Entity { get; set; }

	//	/// <summary>
	//	/// Retreived entity translation
	//	/// </summary>
	//	public TTranslation Translation { get; set; }
	//}	
}