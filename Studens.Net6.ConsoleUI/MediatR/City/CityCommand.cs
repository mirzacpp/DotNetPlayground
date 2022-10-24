using MediatR;

namespace Simplicity.Net6.ConsoleUI.MediatR.City
{
	public class CityResponse
	{
		public string Name { get; set; }
	}

	public class CityCommand : IRequest<CityResponse>
	{
		public string Name { get; set; }
	}

	public class CityCommandHandler : IRequestHandler<CityCommand, CityResponse>
	{
		public Task<CityResponse> Handle(CityCommand request, CancellationToken cancellationToken)
		{
			Console.WriteLine($"Created city {request.Name}");

			return Task.FromResult(new CityResponse { Name = request.Name });
		}
	}
}