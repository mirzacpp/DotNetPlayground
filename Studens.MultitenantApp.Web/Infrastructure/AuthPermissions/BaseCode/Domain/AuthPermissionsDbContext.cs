// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Rev.AuthPermissions.BaseCode.DataLayer.Classes;
using Rev.AuthPermissions.BaseCode.DataLayer.Classes.SupportTypes;

namespace Rev.AuthPermissions.BaseCode.DataLayer.EfCode
{
	/// <summary>
	/// This forms the AuthP's EF Core database
	/// </summary>
	public abstract class AuthPermissionsDbContext : IdentityUserContext<User>
	{
		/// <summary>
		/// This overcomes the exception if the class used in the tests which uses the <see cref="IModelCacheKeyFactory"/>
		/// to allow testing of an DbContext that works with SqlServer and PostgreSQL
		/// </summary>
		public string ProviderName { get; }

		/// <summary>
		/// ctor
		/// </summary>
		/// <param name="options"></param>
		/// <param name="eventSetup">OPTIONAL: If provided, then a method will be run within the ctor</param>
		public AuthPermissionsDbContext(DbContextOptions options,
			IRegisterStateChangeEvent eventSetup = null)
			: base(options)
		{
			eventSetup?.RegisterEventHandlers(this);
			ProviderName = Database.ProviderName;
		}

		/// <summary>
		/// A list of all the AuthP's Roles, each with the permissions in each Role
		/// </summary>
		public DbSet<RoleToPermissions> RoleToPermissions { get; set; }

		/// <summary>
		/// When using AuthP's multi-tenant feature these define each tenant and the DataKey to access data in that tenant
		/// </summary>
		public DbSet<Tenant> Tenants { get; set; }

		/// <summary>
		/// This links AuthP's Roles to a AuthUser
		/// </summary>
		public DbSet<UserToRole> UserToRoles { get; set; }

		/// <summary>
		/// If you use AuthP's JWT refresh token, then the tokens are held in this entity
		/// </summary>
		public DbSet<RefreshToken> RefreshTokens { get; set; }

		/// <summary>
		/// Set up AuthP's setup
		/// </summary>
		/// <param name="modelBuilder"></param>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Invoke Identity ctx configuration
			base.OnModelCreating(modelBuilder);

			//Add concurrency token to every entity
			foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
			{
				if (Database.IsSqlServer())
				{
					entityType.AddProperty("ConcurrencyToken", typeof(byte[]))
						.SetColumnType("ROWVERSION");
					entityType.FindProperty("ConcurrencyToken")
						.ValueGenerated = ValueGenerated.OnAddOrUpdate;
					entityType.FindProperty("ConcurrencyToken")
						.IsConcurrencyToken = true;
				}
				else if (Database.IsNpgsql())
				{
					//see https://www.npgsql.org/efcore/modeling/concurrency.html
					//and https://github.com/npgsql/efcore.pg/issues/19#issuecomment-253346255
					entityType.AddProperty("xmin", typeof(uint))
						.SetColumnType("xid");
					entityType.FindProperty("xmin")
						.ValueGenerated = ValueGenerated.OnAddOrUpdate;
					entityType.FindProperty("xmin")
						.IsConcurrencyToken = true;
				}
				//NOTE: Sqlite doesn't support concurrency support, but if needed it can be added
				//see https://www.bricelam.net/2020/08/07/sqlite-and-efcore-concurrency-tokens.html
			}

			modelBuilder.Entity<User>()
				.ToTable("User", "auth")
				.Property(b => b.Id).IsRequired().HasMaxLength(AuthDbConstants.UserIdSize);
			modelBuilder.Entity<User>().Property(b => b.TenantId).IsRequired(false);
			modelBuilder.Entity<User>().Property(b => b.Email).IsRequired().HasMaxLength(AuthDbConstants.EmailSize);
			modelBuilder.Entity<User>().Property(b => b.NormalizedEmail).IsRequired().HasMaxLength(AuthDbConstants.EmailSize);
			modelBuilder.Entity<User>().Property(b => b.UserName).IsRequired().HasMaxLength(AuthDbConstants.EmailSize);
			modelBuilder.Entity<User>().Property(b => b.NormalizedUserName).IsRequired().HasMaxLength(AuthDbConstants.EmailSize);

			modelBuilder.Entity<User>()
				.HasMany(x => x.UserRoles).WithOne().HasForeignKey(x => x.UserId);

			modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin", "auth");
			modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserToken", "auth");
			modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim", "auth");

			modelBuilder.Entity<RoleToPermissions>()
				.ToTable(nameof(RoleToPermissions), "auth")
				.HasIndex(x => x.RoleType);

			modelBuilder.Entity<UserToRole>()
				.ToTable("UserToRole", "auth")
				.HasKey(x => new { x.UserId, x.RoleName });

			modelBuilder.Entity<Tenant>()
			.HasKey(x => x.TenantId);
			modelBuilder.Entity<Tenant>()
				.ToTable("Tenant", "auth")
				.HasIndex(x => x.TenantName)
				.IsUnique();

			modelBuilder.Entity<Tenant>()
				.HasMany(x => x.TenantRoles)
				.WithMany(x => x.Tenants);

			modelBuilder.Entity<RefreshToken>()
				.ToTable("RefreshToken", "auth")
				.Property(x => x.TokenValue)
				.IsUnicode(false)
				.HasMaxLength(AuthDbConstants.RefreshTokenValueSize)
				.IsRequired();

			modelBuilder.Entity<RefreshToken>()
				.HasKey(x => x.TokenValue);

			modelBuilder.Entity<RefreshToken>()
				.HasIndex(x => x.AddedDateUtc)
				.IsUnique();
		}
	}
}