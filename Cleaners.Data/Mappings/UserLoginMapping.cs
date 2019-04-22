using Cleaners.Core.Constants;
using Cleaners.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cleaners.Data.Mappings
{
    /// <summary>
    /// FluentApi configuration for UserLogin entity
    /// </summary>
    public class UserLoginMapping : IEntityTypeConfiguration<UserLogin>
    {
        public void Configure(EntityTypeBuilder<UserLogin> builder)
        {
            builder.ToTable(TableNames.UserLogins);
        }
    }
}