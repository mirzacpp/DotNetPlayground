using Cleaners.Core.Constants;
using Cleaners.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cleaners.Data.Mappings
{
    /// <summary>
    /// FluentApi configuration for RoleClaim entity
    /// </summary>
    public class RoleClaimMapping : IEntityTypeConfiguration<RoleClaim>
    {
        public void Configure(EntityTypeBuilder<RoleClaim> builder)
        {
            builder.ToTable(TableNames.RoleClaims);
        }
    }
}