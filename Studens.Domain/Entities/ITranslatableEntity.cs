namespace Studens.Domain.Entities
{
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