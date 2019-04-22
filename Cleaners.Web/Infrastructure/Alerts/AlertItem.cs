namespace Cleaners.Web.Infrastructure.Alerts
{
    public class AlertItem
    {
        public string Text { get; set; }
        public string Title { get; set; }
        public bool IsDismissable { get; set; }
        public AlertType Type { get; set; }

        public AlertItem(AlertType type, string text, string title = null, bool isDismissable = true)
        {
            Text = text;
            Title = title;
            IsDismissable = isDismissable;
            Type = type;
        }
    }
}