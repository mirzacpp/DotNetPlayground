using MediatR;

namespace Studens.MvcNet6.WebUI.OutOfProcess
{
	public static class OutOfProcessEndpoints
	{
		public static IEndpointRouteBuilder MapOutOfProcessEndpoints(this IEndpointRouteBuilder builder)
		{
			builder.MapGet("/out/email", async ctx =>
			{
				var service = ctx.RequestServices.GetRequiredService<IMediator>();

				await service.Send(new InviteUser
				{
					Email = "user@email.com",
					Name = "Jon Doe"
				});

				await ctx.Response.WriteAsJsonAsync("All fine", ctx.RequestAborted);
			});

			return builder;
		}
	}
}