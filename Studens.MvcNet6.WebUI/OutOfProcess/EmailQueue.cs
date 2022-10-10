namespace Studens.MvcNet6.WebUI.OutOfProcess
{
	public class EmailQueue
	{
		public long Id { get; set; }
		public string From { get; set; }
		public string To { get; set; }
		public string Subject { get; set; }
		public string Title { get; set; }
		public DateTime? SentAtUtc { get; set; }
		public int Retries { get; set; }
	}
}