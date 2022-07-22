using Studens.Data.Seed;
using Studens.MvcNet6.WebUI.Data;
using Studens.MvcNet6.WebUI.Domain;

namespace Studens.MvcNet6.WebUI.MediatR.Books
{
	public class BooksSeedContributor : IDataSeedContributor
	{
		protected ApplicationDbContext DbContext { get; }

		public BooksSeedContributor(ApplicationDbContext dbContext)
		{
			DbContext = dbContext;
		}

		public async Task SeedDataAsync()
		{
			//var catLocales = new[] { "en", "bs", "fr", "it", "de" };
			var catLocales = new Dictionary<string, string> {
				{"en", "Category {0}" },
				{"bs", "Kategorija {0}" },
				{"fr", "Catégorie {0}" },
				{"it", "Category {0}" },
				{"de", "Categoria {0}" }
			};

			var categories = new List<Category>(1000);

			foreach (var i in Enumerable.Range(1, 1000))
			{
				var cat = new Category
				{
					Locales = catLocales.Select(l => new CategoryLocales
					{
						LanguageCode = l.Key,
						Name = string.Format(l.Value, i)
					}).ToList()
				};

				DbContext.Set<Category>().Add(cat);
				categories.Add(cat);
			}

			await DbContext.SaveChangesAsync();
			var pubs = new List<Publisher>(1000);

			foreach (var i in Enumerable.Range(1, 1000))
			{
				var cat = new Publisher
				{
					Name = "Book publisher " + i
				};

				DbContext.Set<Publisher>().Add(cat);
				pubs.Add(cat);
			}

			await DbContext.SaveChangesAsync();
			var authors = new List<Author>(400);

			foreach (var i in Enumerable.Range(1, 400))
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

			foreach (var i in Enumerable.Range(1, 100_000))
			{
				var book = new Book
				{
					Authors = new List<AuthorBooks> {
						new AuthorBooks {
							AuthorId = authors[random.Next(1, 400)].Id
						},
						new AuthorBooks {
							AuthorId = authors[random.Next(1, 400)].Id
						}
					},
					PublisherId = pubs[random.Next(1, 1000)].Id,
					NumberOfPages = random.Next(99, 510),
					PublishDateTime = new DateTime(random.Next(1650, 2022), random.Next(1, 12), random.Next(1, 28)),
					Price = Convert.ToDecimal(random.Next(10, 50)),
					CategoryId = categories[random.Next(1, 1000)].Id,
					Locales = bookLocales.Select(l => new BookLocales
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