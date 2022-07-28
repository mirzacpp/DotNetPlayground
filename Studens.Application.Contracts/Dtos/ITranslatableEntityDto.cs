namespace Studens.Application.Contracts.Dtos
{
	/// <summary>
	/// Defines DTO with properties for translation.
	/// </summary>
	public interface ITranslatableEntityDto<T>
	{
		public IList<T> Translations { get; set; }
	}
}