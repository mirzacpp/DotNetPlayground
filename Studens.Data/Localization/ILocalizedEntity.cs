namespace Studens.Data.Localization
{
	/// <summary>
	/// Marks entity as localized.
	/// Entity implementing this interface will hold all localized information.
	/// </summary>
	/// <typeparam name="TParentEntity">Parent entity type</typeparam>
	/// <typeparam name="TParentKey">Parent key type</typeparam>
	public interface ILocalizedEntity<TParentEntity, TParentKey>
	{
		/// <summary>
		/// Represents locales language code
		/// TODO: Replace with shadow property. Check provider support?
		/// </summary>
		string LanguageCode { get; set; }

		/// <summary>
		/// Parents navigation property
		/// </summary>
		public TParentEntity Parent { get; set; }

		public TParentKey ParentId { get; set; }
	}

	/// <summary>
	/// Predefined marker where <see cref="TParentEntity"/> has an integer PK.
	/// </summary>
	/// <typeparam name="TParentEntity">Parent entity type</typeparam>
	public interface ILocalizedEntity<TParentEntity> : ILocalizedEntity<TParentEntity, int>
	{
	}

	/// <summary>
	/// Indicates that implementing entity has localized content.
	/// </summary>
	/// <typeparam name="TEntity">Entity type</typeparam>
	public interface IHasLocales<TEntity>
	{
		ICollection<TEntity> Locales { get; set; }
	}
}