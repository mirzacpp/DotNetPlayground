using Studens.Data.Seed;
using Studens.MvcNet6.WebUI.Data;
using Studens.MvcNet6.WebUI.Domain;

namespace Studens.MvcNet6.WebUI.MediatR.Books
{
	[DataSeed(Environment = "Development")]
	public class BooksSeedContributor : IDataSeedContributor
	{
		protected ApplicationDbContext DbContext { get; }
		public int Order { get; }

		public BooksSeedContributor(ApplicationDbContext dbContext)
		{
			DbContext = dbContext;
		}

		public async Task SeedDataAsync()
		{
			//var catLocales = new[] { "en", "bs", "fr", "it", "de" };
			var catLocales = new Dictionary<string, string> {
				{"en-GB", "Category {0}" },
				{"en-US", "Category {0}" },
				{"bs", "Kategorija {0}" },
				{"fr", "Catégorie {0}" },
				{"it", "Category {0}" },
				{"de", "Categoria {0}" }
			};

			var categories = new List<Category>(50);

			foreach (var i in Enumerable.Range(1, 50))
			{
				var cat = new Category
				{
					Translations = catLocales.Select(l => new CategoryLocales
					{
						LanguageCode = l.Key,
						Name = string.Format(l.Value, i)
					}).ToList()
				};

				DbContext.Set<Category>().Add(cat);
				categories.Add(cat);
			}

			await DbContext.SaveChangesAsync();
			var pubs = new List<Publisher>(10);

			foreach (var i in Enumerable.Range(1, 10))
			{
				var cat = new Publisher
				{
					Name = "Book publisher " + i
				};

				DbContext.Set<Publisher>().Add(cat);
				pubs.Add(cat);
			}

			await DbContext.SaveChangesAsync();
			var authors = new List<Author>(20);

			foreach (var i in Enumerable.Range(1, 20))
			{
				var aut = new Author
				{
					FirstName = "Author " + i,
					LastName = "Author " + i
				};

				DbContext.Set<Author>().Add(aut);
				authors.Add(aut);
			}

			await DbContext.SaveChangesAsync();

			var bookLocales = new Dictionary<string, string> {
				{"en", "Book {0}" },
				{"bs", "Knjiga {0}" },
				{"fr", "Livre {0}" },
				{"it", "Libro {0}" },
				{"de", "Buch {0}" }
			};

			var random = new Random();

			foreach (var i in Enumerable.Range(1, 200))
			{
				var book = new Book
				{
					Authors = new List<AuthorBooks> {
						new AuthorBooks {
							AuthorId = authors[random.Next(1, 20)].Id
						},
						new AuthorBooks {
							AuthorId = authors[random.Next(1, 20)].Id
						}
					},
					PublisherId = pubs[random.Next(1, 10)].Id,
					NumberOfPages = random.Next(99, 510),
					PublishDateTime = new DateTime(random.Next(1650, 2022), random.Next(1, 12), random.Next(1, 28)),
					Price = Convert.ToDecimal(random.Next(10, 50)),
					CategoryId = categories[random.Next(1, 50)].Id,
					Translations = bookLocales.Select(l => new BookLocales
					{
						LanguageCode = l.Key,
						Title = string.Format(l.Value, i)
					}).ToList()
				};

				DbContext.Set<Book>().Add(book);
			}
		}
	}

	[DataSeed(Environment = "Production")]
	public class BooksSeedContributorProduction : IDataSeedContributor
	{
		protected ApplicationDbContext DbContext { get; }
		public int Order { get; }

		public BooksSeedContributorProduction(ApplicationDbContext dbContext)
		{
			DbContext = dbContext;
		}

		public async Task SeedDataAsync()
		{
			//var catLocales = new[] { "en", "bs", "fr", "it", "de" };
			var catLocales = new Dictionary<string, string> {
				{"en-GB", "Category {0}" },				
				{"de", "Categoria {0}" }
			};

			var categories = new List<Category>(50);

			foreach (var i in Enumerable.Range(1, 50))
			{
				var cat = new Category
				{
					Translations = catLocales.Select(l => new CategoryLocales
					{
						LanguageCode = l.Key,
						Name = string.Format(l.Value, i)
					}).ToList()
				};

				DbContext.Set<Category>().Add(cat);
				categories.Add(cat);
			}

			await DbContext.SaveChangesAsync();
			var pubs = new List<Publisher>(10);

			foreach (var i in Enumerable.Range(1, 10))
			{
				var cat = new Publisher
				{
					Name = "Book publisher " + i
				};

				DbContext.Set<Publisher>().Add(cat);
				pubs.Add(cat);
			}

			await DbContext.SaveChangesAsync();
			var authors = new List<Author>(20);

			foreach (var i in Enumerable.Range(1, 20))
			{
				var aut = new Author
				{
					FirstName = "Author " + i,
					LastName = "Author " + i
				};

				DbContext.Set<Author>().Add(aut);
				authors.Add(aut);
			}

			await DbContext.SaveChangesAsync();

			var bookLocales = new Dictionary<string, string> {
				{"en", "Book {0}" },				
				{"de", "Buch {0}" }
			};

			var random = new Random();

			foreach (var i in Enumerable.Range(1, 200))
			{
				var book = new Book
				{
					Authors = new List<AuthorBooks> {
						new AuthorBooks {
							AuthorId = authors[random.Next(1, 20)].Id
						},
						new AuthorBooks {
							AuthorId = authors[random.Next(1, 20)].Id
						}
					},
					PublisherId = pubs[random.Next(1, 10)].Id,
					NumberOfPages = random.Next(99, 510),
					PublishDateTime = new DateTime(random.Next(1650, 2022), random.Next(1, 12), random.Next(1, 28)),
					Price = Convert.ToDecimal(random.Next(10, 50)),
					CategoryId = categories[random.Next(1, 50)].Id,
					Translations = bookLocales.Select(l => new BookLocales
					{
						LanguageCode = l.Key,
						Title = string.Format(l.Value, i)
					}).ToList()
				};

				DbContext.Set<Book>().Add(book);
			}
		}
	}
}