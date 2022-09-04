using System.Security.Claims;

namespace Studens.MvcNet6.WebUI.MultiTenant
{
	public static class ClaimPrincipalExtensions
	{
		public static string GetTenantId(this ClaimsPrincipal claimsPrincipal)
		{
			return claimsPrincipal.Claims.SingleOrDefault(c => c.Type == "TenantId")?.Value;
		}

		public static string GetDatabaseInfoName(this ClaimsPrincipal claimsPrincipal)
		{
			return claimsPrincipal.Claims.SingleOrDefault(c => c.Type == "DbInfoName")?.Value;
		}
	}
}