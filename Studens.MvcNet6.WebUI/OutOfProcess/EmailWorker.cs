using MediatR;
using Simplicity.MvcNet6.WebUI.Data;

namespace Simplicity.MvcNet6.WebUI.OutOfProcess
{
	public class EmailWorker : INotificationHandler<UserInvited>
	{
		private readonly IEmailSender _emailSender;
		private readonly ApplicationDbContext _context;

		public EmailWorker(IEmailSender emailSender, ApplicationDbContext context)
		{
			_emailSender = emailSender;
			_context = context;
		}

		public async Task Handle(UserInvited notification, CancellationToken cancellationToken)
		{
			var email = new EmailQueue
			{
				From = "app@app.com",
				Subject = "Invitation for " + notification.Email,
				Title = "Invitation for " + notification.Email,
				To = notification.Email
			};

			_context.EmailQueue.Add(email);
			await _context.SaveChangesAsync();

			//await _emailSender.SendEmailAsync(new EmailMessage
			//{
			//	To = notification.Email,
			//	From = "Best app in the world",
			//	Subject = $"Message for {notification.Email}",
			//	Message = $"Message content"
			//});

			//email.SentAtUtc = DateTime.UtcNow;
			//await _context.SaveChangesAsync();
		}
	}
}