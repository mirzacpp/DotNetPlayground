using Studens.Commons.Extensions;

namespace Studens.MvcNet6.WebUI.MultiTenant
{
	public interface ICurrentTenant
	{
		public string TenantId { get; }
		public string ConnectionString { get; }
	}

	public class ClaimCurrentTenantResolver : ICurrentTenant
	{
		public string TenantId { get; }
		public string ConnectionString { get; }

		public ClaimCurrentTenantResolver(IHttpContextAccessor httpContextAccessor)
		{
			TenantId = httpContextAccessor.HttpContext?.User.GetTenantId();
			var databaseInfoName = httpContextAccessor.HttpContext?.User.GetDatabaseInfoName();

			if (databaseInfoName.IsNotNullOrEmpty())
			{
				// Reolsve our connection string here
				ConnectionString = databaseInfoName;
			}
		}
	}
}