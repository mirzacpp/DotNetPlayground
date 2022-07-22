using MediatR;
using Studens.MvcNet6.WebUI.MediatR.Books;
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

			_ = app.MapGet("/books/{lang}/{page}/{pageSize}", async (HttpContext context, string lang, int page, int pageSize) =>
			{
				var mediatR = context.RequestServices.GetRequiredService<IMediator>();

				await context.Response.WriteAsJsonAsync(await mediatR.Send(new BooksQuery
				{
					LangCode = lang,
					Page = page,
					PageSize = pageSize
				}));
			});

			return app;
		}
	}
}