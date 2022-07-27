using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Studens.MvcNet6.WebUI.Domain
{
	//public abstract class LocalizedEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
	//where TEntity : class, ILocalizedEntity
	//{
	//	public void Configure(EntityTypeBuilder<TEntity> builder)
	//	{
	//		builder.ToTable(nameof(TEntity));
	//		builder.HasKey()
	//	}
	//}

	public class BookConfiguration : IEntityTypeConfiguration<Book>
	{
		public void Configure(EntityTypeBuilder<Book> builder)
		{
			builder.ToTable(nameof(Book));
			builder.Property(b => b.Price).IsRequired();

			//TODO: This can now be auto configured ?
			builder.HasMany(b => b.Authors).WithOne(bl => bl.Book).HasForeignKey(bl => bl.BookId);
		}
	}

	public class BookLocalesConfiguration : TranslatableEntityConfiguration<BookLocales, Book, int>
	{
		public void Configure(EntityTypeBuilder<BookLocales> builder)
		{
			base.Configure(builder);
			builder.HasKey(b => b.Id);
			builder.Property(b => b.Title).IsRequired();
		}
	}

	public class AuthorConfiguration : IEntityTypeConfiguration<Author>
	{
		public void Configure(EntityTypeBuilder<Author> builder)
		{
			builder.ToTable(nameof(Author));
			builder.HasKey(b => b.Id);

			//TODO: This can now be auto configured ?
			builder.HasMany(b => b.Books).WithOne(bl => bl.Author).HasForeignKey(bl => bl.AuthorId);
		}
	}

	public class PublisherConfiguration : IEntityTypeConfiguration<Publisher>
	{
		public void Configure(EntityTypeBuilder<Publisher> builder)
		{
			builder.ToTable(nameof(Publisher));
			builder.HasKey(b => b.Id);

			//TODO: This can now be auto configured ?
			builder.HasMany(b => b.Books).WithOne(bl => bl.Publisher).HasForeignKey(bl => bl.PublisherId);
		}
	}

	public class CategoryConfiguration : IEntityTypeConfiguration<Category>
	{
		public void Configure(EntityTypeBuilder<Category> builder)
		{
			builder.ToTable(nameof(Category));
			builder.HasKey(b => b.Id);

			//TODO: This can now be auto configured ?
			builder.HasMany(b => b.Books).WithOne(b => b.Category).HasForeignKey(b => b.CategoryId);
		}
	}

	public class CategoryLocalesConfiguration : TranslatableEntityConfiguration<CategoryLocales, Category, int>
	{
		public void Configure(EntityTypeBuilder<CategoryLocales> builder)
		{
			base.Configure(builder);

			// No need for primary key?
			builder.HasKey(b => b.Id);
			// To avoid scnearios when translation is not present, we will allow to store null records.
			// This way we won't break paging, and we can post populate with default language if neccessary.
			builder.Property(b => b.Name).IsRequired(false);
		}
	}
}