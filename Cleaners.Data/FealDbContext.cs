using Cleaners.Core.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cleaners.Data
{
    /// <summary>
    /// DbContext implementation
    /// </summary>
    public class FealDbContext
        : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public FealDbContext(DbContextOptions<FealDbContext> options)
            : base(options)
        {
        }

        // Your DbSets here

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Apply base model configurations
            base.OnModelCreating(builder);

            // Load entity configurations from this assembly
            // This line has to go after the base OnModelCreating call
            builder.ApplyConfigurationsFromAssembly(typeof(FealDbContext).Assembly);
        }
    }
}