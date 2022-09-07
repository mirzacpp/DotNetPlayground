using MediatR;
using Studens.MediatR;

namespace Studens.MultitenantApp.Web.Features
{
	public class InviteTeamMemberRequest : ICommand<Unit>
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}

	public class InviteTeamMemberHandler : IRequestHandler<InviteTeamMemberRequest, Unit>
	{
		public async Task<Unit> Handle(InviteTeamMemberRequest request, CancellationToken cancellationToken)
		{
			return Unit.Value;
		}
	}
}