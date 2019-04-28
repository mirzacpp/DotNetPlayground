using Newtonsoft.Json;
using System.Collections.Generic;

namespace Cleaners.Web.Infrastructure.Alerts
{
    [JsonObject]
    public class AlertList : List<AlertItem>
    {
        public void Success(string text, string title = null, bool isDismissable = true)
        {
            Add(new AlertItem(AlertType.Success, text, title, isDismissable));
        }

        public void Danger(string text, string title = null, bool isDismissable = true)
        {
            Add(new AlertItem(AlertType.Danger, text, title, isDismissable));
        }

        public void Warning(string text, string title = null, bool isDismissable = true)
        {
            Add(new AlertItem(AlertType.Warning, text, title, isDismissable));
        }

        public void Info(string text, string title = null, bool isDismissable = true)
        {
            Add(new AlertItem(AlertType.Info, text, title, isDismissable));
        }

        public static AlertList Empty => new AlertList();
    }
}