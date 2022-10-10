namespace Studens.Application.Contracts.Dtos
{
	/// <summary>
	/// Default implementation of <see cref="IListResult{T}"/>
	/// </summary>
	/// <typeparam name="T">Type of the items</typeparam>
	[Serializable]
	public class ListResultDto<T> : IListResult<T>
	{
		private IReadOnlyList<T> _items;

		public IReadOnlyList<T> Items
		{
			get => _items ??= new List<T>();
			set => _items = value;
		}

		public ListResultDto()
		{
		}

		public ListResultDto(List<T> items)
		{
			Items = Items;
		}
	}
}