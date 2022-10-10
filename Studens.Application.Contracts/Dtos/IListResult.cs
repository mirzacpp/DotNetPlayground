namespace Studens.Application.Contracts.Dtos
{
	/// <summary>
	/// Introduces standardized contracts for collection responses.
	/// </summary>
	/// <typeparam name="T">Type of the items</typeparam>
	public interface IListResult<T>
	{
		IReadOnlyList<T> Items { get; set; }
	}
}