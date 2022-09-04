using Studens.Domain.Entities;

namespace Studens.MvcNet6.WebUI.MultiTenant.Demo
{
	public class Vehicles : IEntity<long>, IMultiTenant
	{
		public long Id { get; set; }
		public string Model { get; set; }
		public string? TenantId { get; set; }
	}
}