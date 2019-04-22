namespace Cleaners.Web.Infrastructure.Alerts
{
    public class AlertManager : IAlertManager
    {
        public AlertManager()
        {
            Alerts = new AlertList();
        }

        public AlertList Alerts { get; }
    }
}