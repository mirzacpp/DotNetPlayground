using Microsoft.EntityFrameworkCore;

namespace Simplicity.Api.Domain
{
    public class SimpleDbContext : DbContext
    {
        public SimpleDbContext(DbContextOptions<SimpleDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Author>().HasKey(x => x.Id);
            modelBuilder.Entity<Author>()
                .HasMany(x => x.Books)
                .WithOne(x => x.Author)
                .HasForeignKey(x => x.AuthorId);

            modelBuilder.Entity<Book>().HasKey(x => x.Id);

            modelBuilder.Entity<Author>()
                .HasData(
                new Author
                {
                    Id = 1,
                    Name = "Albert Camus"
                },
                new Author
                {
                    Id = 2,
                    Name = "Arthur Conan Doyle"
                });

            modelBuilder.Entity<Book>().HasData(new[] {
                    new Book { Id = 1, Title = "The Stranger", Pages = 250, AuthorId = 1 },
                    new Book { Id = 2, Title = "The Plague", Pages = 250, AuthorId = 1 },
                    new Book { Id = 3, Title = "A Study in Scarlet", Pages = 250, AuthorId = 2 },
                    new Book { Id = 4, Title = "The Hound of the Baskervilles", Pages = 250, AuthorId = 2 }
            });
        }
    }
}