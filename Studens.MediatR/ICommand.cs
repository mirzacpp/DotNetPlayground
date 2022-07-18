using MediatR;

namespace Studens.MediatR
{
	/// <summary>
	///	Marker interface to represent a command request.
	/// </summary>
	/// <typeparam name="TResponse">Response type</typeparam>
	public interface ICommand<out TResponse> : IRequest<TResponse>
	{
	}
}