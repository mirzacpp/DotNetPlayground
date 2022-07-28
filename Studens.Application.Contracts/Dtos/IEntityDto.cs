namespace Studens.Application.Contracts.Dtos
{
	/// <summary>
	/// Predefines <see cref="IEntityDto{TKey}"/> using <see cref="int"/> as type.
	/// </summary>
	public interface IEntityDto : IEntityDto<int>
	{
	}

	/// <summary>
	/// Defines DTO with Id property.
	/// </summary>
	/// <typeparam name="TKey">Id type</typeparam>
	public interface IEntityDto<TKey>
	{
		public TKey Id { get; set; }
	}
}