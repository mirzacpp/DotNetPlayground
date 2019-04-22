using Cleaners.Core.Constants;
using Cleaners.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cleaners.Data.Mappings
{
    /// <summary>
    /// FluentApi configuration for User entity
    /// </summary>
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(TableNames.Users);

            // By default Identity doesn't provide navigation properties, so manual configuration is required
            builder.HasMany(u => u.Claims).WithOne(c => c.User).HasForeignKey(uc => uc.UserId).IsRequired();
            builder.HasMany(u => u.Logins).WithOne(c => c.User).HasForeignKey(uc => uc.UserId).IsRequired();
            builder.HasMany(u => u.Tokens).WithOne(c => c.User).HasForeignKey(uc => uc.UserId).IsRequired();
            builder.HasMany(u => u.UserRoles).WithOne(c => c.User).HasForeignKey(uc => uc.UserId).IsRequired();

            builder.Property(u => u.FirstName).IsRequired(false).HasMaxLength(250);
            builder.Property(u => u.LastName).IsRequired(false).HasMaxLength(250);
            builder.Property(u => u.IsActive).IsRequired();
            builder.Property(u => u.CreationDateUtc).IsRequired();
            builder.Property(u => u.LastUpdateDateUtc).IsRequired(false);
        }
    }
}