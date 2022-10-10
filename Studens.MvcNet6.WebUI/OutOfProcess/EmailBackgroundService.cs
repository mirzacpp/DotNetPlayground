using Microsoft.EntityFrameworkCore;
using Studens.MvcNet6.WebUI.Data;
using System.Text.Json;

namespace Studens.MvcNet6.WebUI.OutOfProcess
{
	public class EmailBackgroundService : BackgroundService
	{
		private readonly IServiceScopeFactory _serviceScopeFactory;

		public EmailBackgroundService(IServiceScopeFactory serviceScopeFactory)
		{
			_serviceScopeFactory = serviceScopeFactory;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			Console.WriteLine($"Starting email background service on thread: {Environment.CurrentManagedThreadId}");

			var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));
			using var sp = _serviceScopeFactory.CreateScope();
			var emailSender = sp.ServiceProvider.GetRequiredService<IEmailSender>();
			var dbContext = sp.ServiceProvider.GetRequiredService<ApplicationDbContext>();			

			while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
			{
				Console.WriteLine($"Executing task on thread: {Environment.CurrentManagedThreadId}");

				var emailsToSend = await dbContext.EmailQueue
				.Where(e => !e.SentAtUtc.HasValue)
				//.OrderBy(e => e.Crea)
				.ToListAsync(stoppingToken);

				foreach (var item in emailsToSend)
				{
					try
					{
						await emailSender.SendEmailAsync(new EmailMessage
						{
							To = item.To,
							From = "Best app in the world",
							Subject = $"Message for {item.To}",
							Message = $"Message content"
						});

						item.SentAtUtc = DateTime.UtcNow;
						await dbContext.SaveChangesAsync();
					}
					catch (Exception ex)
					{
						item.Retries += 1;
						await dbContext.SaveChangesAsync();

						throw;
					}
				}
			}

			Console.WriteLine($"Stopping email background service on thread: {Environment.CurrentManagedThreadId}");
		}
	}
}