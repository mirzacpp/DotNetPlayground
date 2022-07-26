namespace Studens.Data.Localization
{
	public interface IEntityTranslation
	{
		/// <summary>
		/// Represents translation language code.
		/// </summary>
		string LanguageCode { get; set; }
	}

	/// <summary>
	/// Marks entity as localized.
	/// Entity implementing this interface will store all translatable data.
	/// </summary>
	/// <typeparam name="TTranslatableEntity">Parent entity type</typeparam>
	/// <typeparam name="TTranslatableEntityKey">Parent key type</typeparam>
	public interface IEntityTranslation<TTranslatableEntity, TTranslatableEntityKey> : IEntityTranslation
	{
		/// <summary>
		/// Parents navigation property
		/// </summary>
		public TTranslatableEntity Parent { get; set; }

		/// <summary>
		/// Parents FK property
		/// </summary>
		public TTranslatableEntityKey ParentId { get; set; }
	}

	/// <summary>
	/// Predefined marker where <see cref="TEntity"/> has an integer PK.
	/// </summary>
	/// <typeparam name="TEntity">Parent entity type</typeparam>
	public interface IEntityTranslation<TEntity> : IEntityTranslation<TEntity, int>
	{
	}

	/// <summary>
	/// Indicates that implementing entity has localized content.
	/// </summary>
	/// <typeparam name="TTranslation">Translatable entity type</typeparam>
	public interface ITranslatableEntity<TTranslation> where TTranslation : class, IEntityTranslation
	{
		/// <summary>
		/// Defines list of translations for entity of type <see cref="TTranslation"/>.
		/// </summary>
		ICollection<TTranslation> Translations { get; set; }
	}
}