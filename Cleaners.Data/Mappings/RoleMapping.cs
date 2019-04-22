using Cleaners.Core.Constants;
using Cleaners.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cleaners.Data.Mappings
{
    /// <summary>
    /// FluentApi configuration for Role entity
    /// </summary>
    public class RoleMapping : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable(TableNames.Roles);

            // By default Identity doesn't provide navigation properties, so manual configuration is required
            builder.HasMany(u => u.UserRoles).WithOne(c => c.Role).HasForeignKey(rc => rc.RoleId).IsRequired();
            builder.HasMany(u => u.RoleClaims).WithOne(c => c.Role).HasForeignKey(rc => rc.RoleId).IsRequired();
        }
    }
}