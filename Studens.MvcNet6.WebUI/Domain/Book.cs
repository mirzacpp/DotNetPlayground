using Studens.Data.Localization;
using Studens.Domain.Entities;

namespace Studens.MvcNet6.WebUI.Domain
{
	/// <summary>
	/// Defines base entity
	/// </summary>
	public class Book : IEntity<int>, IHasLocales<BookLocales>
	{
		public Book()
		{
			Locales = new HashSet<BookLocales>();
		}

		public int Id { get; set; }
		public decimal Price { get; set; }
		public DateTime PublishDateTime { get; set; }
		public int NumberOfPages { get; set; }
		public int CategoryId { get; set; }
		public int PublisherId { get; set; }

		public Category Category { get; set; }
		public Publisher Publisher { get; set; }
		public ICollection<BookLocales> Locales { get; set; }
		public ICollection<AuthorBooks> Authors { get; set; }
	}

	// Holds all translatable properties
	public class BookLocales : ILocalizedEntity<Book, int>
	{
		public int Id { get; set; }
		public string LanguageCode { get; set; }
		public string Title { get; set; }

		public int ParentId { get; set; }
		public Book Parent { get; set; }
	}

	public class AuthorBooks : IEntity<int>
	{
		public int Id { get; set; }
		public int AuthorId { get; set; }
		public int BookId { get; set; }

		public Author Author { get; set; }
		public Book Book { get; set; }
	}

	public class Author : IEntity<int>
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }

		public ICollection<AuthorBooks> Books { get; set; }
	}

	public class Publisher : IEntity<int>
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public ICollection<Book> Books { get; set; }
	}

	public class Category : IEntity<int>, IHasLocales<CategoryLocales>
	{
		public Category()
		{
			Locales = new HashSet<CategoryLocales>();
		}

		public int Id { get; set; }
		public ICollection<CategoryLocales> Locales { get; set; }
		public ICollection<Book> Books { get; set; }
	}

	// Holds all translatable properties
	public class CategoryLocales : ILocalizedEntity<Category, int>
	{
		public int Id { get; set; }
		public string LanguageCode { get; set; }
		public string Name { get; set; }

		public int ParentId { get; set; }
		public Category Parent { get; set; }
	}
}