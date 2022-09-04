namespace Studens.MvcNet6.WebUI.MultiTenant
{
	public interface IMultiTenant
	{
		string? TenantId { get; set; }
	}

	public interface IMultiTenantReadOnly
	{
		string TenantId { get; }
	}
}