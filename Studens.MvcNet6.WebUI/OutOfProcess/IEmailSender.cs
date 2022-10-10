namespace Studens.MvcNet6.WebUI.OutOfProcess
{
	public interface IEmailSender
	{
		Task SendEmailAsync(EmailMessage message);
	}

	public class DumbEmailSender : IEmailSender
	{
		public async Task SendEmailAsync(EmailMessage message)
		{
			await Task.Delay(4000);
			Console.WriteLine($"Sending email from {message.From} to {message.To} with subject {message.Subject}");
		}
	}

	public class EmailMessage
	{
		public string From { get; set; }
		public string To { get; set; }
		public string Subject { get; set; }
		public string Message { get; set; }
	}
}