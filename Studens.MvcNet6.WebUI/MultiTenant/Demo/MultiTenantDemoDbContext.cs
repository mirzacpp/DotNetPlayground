using Microsoft.EntityFrameworkCore;

namespace Studens.MvcNet6.WebUI.MultiTenant.Demo
{
	public class MultiTenantDemoDbContext : DbContext, IMultiTenantReadOnly
	{
		/// <summary>
		/// Current user tenant id
		/// </summary>
		public string TenantId { get; }

		public MultiTenantDemoDbContext(DbContextOptions<MultiTenantDemoDbContext> options) : base(options)
		{
			
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}
	}
}