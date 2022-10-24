using MediatR;
using Simplicity.MediatR;

namespace Simplicity.MvcNet6.WebUI.OutOfProcess
{
	public class InviteUser : ICommand<Unit>
	{
		public string Email { get; set; }
		public string Name { get; set; }
	}

	public class UserInvited : INotification
	{
		public UserInvited(string email)
		{
			Email = email;
		}

		public string Email { get; }
	}

	public class InviteUserHandler : IRequestHandler<InviteUser, Unit>
	{
		private readonly IMediator _mediator;

		public InviteUserHandler(IMediator mediator)
		{
			_mediator = mediator;
		}

		public async Task<Unit> Handle(InviteUser request, CancellationToken cancellationToken)
		{
			await Task.Delay(3000, cancellationToken);
			Console.WriteLine($"User {request.Name} has been saved to database.");				

			await _mediator.Publish(new UserInvited(request.Email), cancellationToken);

			return Unit.Value;
		}
	}
}