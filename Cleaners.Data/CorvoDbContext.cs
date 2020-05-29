using Cleaners.Core.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cleaners.Data
{
    /// <summary>
    /// DbContext implementation
    /// </summary>
    public class CorvoDbContext
        : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public CorvoDbContext(DbContextOptions<CorvoDbContext> options)
            : base(options)
        {
        }

        // Your DbSets here
        public DbSet<Book> Books { get; set; }

        public DbSet<Author> Authors { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Apply base model configurations
            base.OnModelCreating(builder);

            // Load entity configurations from this assembly
            // This line has to go after the base OnModelCreating call
            builder.ApplyConfigurationsFromAssembly(typeof(CorvoDbContext).Assembly);

            // Configure test book entity
            builder.Entity<Book>().HasKey(c => c.Id);
            builder.Entity<Book>().Property(c => c.Title).IsRequired();
            builder.Entity<Book>().Property(c => c.Pages).IsRequired();
            builder.Entity<Book>().Property(c => c.IsDeleted).HasDefaultValue(false);

            builder.Entity<Book>()
                .HasOne(c => c.Offer)
                .WithOne(o => o.Book)
                .HasForeignKey<BookOffer>(o => o.BookId);

            // Configure test author entity
            builder.Entity<Author>().HasKey(c => c.Id);
            builder.Entity<Author>().Property(c => c.FirstName).IsRequired();
            builder.Entity<Author>().Property(c => c.LastName).IsRequired();

            // Configure Author/Book M:M
            builder.Entity<AuthorBook>().HasKey(c => new { c.AuthorId, c.BookId });
            builder.Entity<AuthorBook>()
                .HasOne(c => c.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(c => c.AuthorId);

            builder.Entity<AuthorBook>()
                .HasOne(c => c.Book)
                .WithMany(a => a.Authors)
                .HasForeignKey(c => c.BookId);
        }
    }
}