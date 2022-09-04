namespace Studens.MvcNet6.WebUI.MultiTenant.Domain
{
	public class Tenant
	{
		public string Id { get; set; }
		public string DatabaseInfoName { get; set; }
		public bool OwnsDatabase { get; set; }
	}
}