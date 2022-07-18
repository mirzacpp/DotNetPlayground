namespace Studens.MediatR
{
	/// <summary>
	/// Represents a paged query request.
	/// </summary>
	/// <typeparam name="TResponse">Response type</typeparam>
	public interface IPagedQuery<out TResponse> : IQuery<TResponse>
	{
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
	}

	/// <summary>
	/// Base implementation for <see cref="IPagedQuery{TResponse}"/>
	/// </summary>
	/// <typeparam name="TResponse">Response type</typeparam>
	public abstract class PagedQuery<TResponse> : IPagedQuery<TResponse>
	{
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
	}
}