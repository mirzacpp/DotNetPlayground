using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Plugins.Http.CSharp;
using Studens.Commons.Collections;

var httpFactory = HttpClientFactory.Create();

var step = Step.Create("stresiraj_api", httpFactory, async ctx =>
{
	var response = await ctx.Client.GetAsync("https://localhost:7132/books/it/107/5", ctx.CancellationToken);

	return response.IsSuccessStatusCode
	? Response.Ok(statusCode: (int)response.StatusCode)
	: Response.Fail(statusCode: (int)response.StatusCode);
});

var scenario = ScenarioBuilder.CreateScenario("test_scene", step)
.WithWarmUpDuration(TimeSpan.FromSeconds(5))
.WithLoadSimulations(Simulation.KeepConstant(15, during: TimeSpan.FromSeconds(60)));

NBomberRunner.RegisterScenarios(scenario).Run();