namespace Cleaners.Web.Infrastructure.Alerts
{
    public interface IAlertManager
    {
        AlertList Alerts { get; }
    }
}