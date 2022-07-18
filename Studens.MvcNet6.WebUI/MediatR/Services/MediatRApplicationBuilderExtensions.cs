using MediatR;
using Studens.MediatR;
using Studens.MvcNet6.WebUI.MediatR.Services;

namespace Microsoft.AspNetCore.Builder
{
	public static class MediatRApplicationBuilderExtensions
	{
		public static IEndpointRouteBuilder UseMediatRTestEndpoints(this IEndpointRouteBuilder app)
		{
			_ = app.MapGet("/mediatr-get", async (context) =>
			  {
				  var mediatR = context.RequestServices.GetRequiredService<IMediator>();

				  await context.Response.WriteAsJsonAsync(await mediatR.Send(new GetCustomerByIdQuery()));
			  });

			_ = app.MapGet("/mediatr-post", async (context) =>
			{
				var mediatR = context.RequestServices.GetRequiredService<IMediator>();

				await context.Response.WriteAsJsonAsync(await mediatR.Send(new CreateCommand<CustomerCreateDto, string>(new CustomerCreateDto
				{
					FirstName = "Jone",
					LastName = "Doe"
				})));
			});

			return app;
		}
	}
}