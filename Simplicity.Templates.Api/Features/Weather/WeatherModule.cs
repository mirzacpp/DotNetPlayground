namespace Simplicity.Templates.Api.Features.Weather;

public class WeatherModule : ICarterModule
{
	private static readonly string[] Summaries = new[]
	{
		"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
	};

	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app
		.MapGet("/weather", () =>
		{
			return Results.Ok(Enumerable.Range(1, 5).Select(index => new WeatherForecast
			{
				Date = DateTime.Now.AddDays(index),
				TemperatureC = Random.Shared.Next(-20, 55),
				Summary = Summaries[Random.Shared.Next(Summaries.Length)]
			})
			.ToArray());
		})
		.WithName("GetWeatherForecast")		
		.IncludeInOpenApi();
	}
}