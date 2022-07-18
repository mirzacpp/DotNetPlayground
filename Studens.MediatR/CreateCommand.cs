using MediatR;
using Microsoft.Extensions.Logging;

namespace Studens.MediatR
{
	/// <summary>
	/// Generic command request used for creating resources.
	/// </summary>
	/// <typeparam name="TResponse"></typeparam>
	public class CreateCommand<TInput, TResponse> : ICommand<TResponse>
	{
		public CreateCommand(TInput input)
		{
			Input = input;
		}

		/// <summary>
		/// Represents an input model.
		/// </summary>
		public TInput Input { get; }
	}

	/// <summary>
	/// Generic command request used for creating resources.
	/// </summary>
	/// <typeparam name="TResponse"></typeparam>
	public class CreateCommand<TInput> : CreateCommand<TInput, Unit>
	{
		public CreateCommand(TInput input)
		: base(input)
		{
		}

		/// <summary>
		/// Represents an input model.
		/// </summary>
		public TInput Input { get; set; }
	}

	public class DummyCreateHandler<TInput, TResponse> : IRequestHandler<CreateCommand<TInput, TResponse>, TResponse>
	
	{
		private readonly ILogger<DummyCreateHandler<TInput, TResponse>> _logger;

		public DummyCreateHandler(ILogger<DummyCreateHandler<TInput, TResponse>> logger)
		{
			_logger = logger;
		}

		public Task<TResponse> Handle(CreateCommand<TInput, TResponse> request, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Creating input of type {request.Input.GetType()}");

			return Task.FromResult(default(TResponse));
		}
	}
}