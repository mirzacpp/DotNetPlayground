using MediatR;

namespace Simplicity.MediatR
{
	/// <summary>
	///	Marker interface to represent a query request.
	/// </summary>
	/// <typeparam name="TResponse">Response type</typeparam>
	public interface IQuery<out TResponse> : IRequest<TResponse>
	{
	}
}