using Cleaners.Core.Constants;
using Cleaners.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cleaners.Data.Mappings
{
    /// <summary>
    /// FluentApi configuration for UserRole entity
    /// </summary>
    public class UserRoleMapping : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable(TableNames.UserRoles);
        }
    }
}