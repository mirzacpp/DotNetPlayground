using MediatR;
using Microsoft.EntityFrameworkCore;
using Studens.Data.EntityFrameworkCore;
using Studens.MediatR;
using Studens.MvcNet6.WebUI.Data;
using Studens.MvcNet6.WebUI.Domain;

namespace Studens.MvcNet6.WebUI.MediatR.Books
{
	public class PublisherDto
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}

	public class BookDto
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public decimal Price { get; set; }
		public DateTime PublishDateTime { get; set; }
		public int NumberOfPages { get; set; }
		public string Category { get; set; }
		public PublisherDto Publisher { get; set; }
	}

	public class BooksQuery : IQuery<IList<BookDto>>
	{
		public int Page { get; set; }
		public int PageSize { get; set; }
		public string LangCode { get; set; }
	}

	public class BookHandler : IRequestHandler<BooksQuery, IList<BookDto>>
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly ILogger<BookHandler> _logger;

		public BookHandler(ApplicationDbContext dbContext, ILogger<BookHandler> logger)
		{
			_dbContext = dbContext;
			_logger = logger;
		}

		private DbSet<Book> Books => _dbContext.Set<Book>();
		private DbSet<BookLocales> BookLocales => _dbContext.Set<BookLocales>();

		public async Task<IList<BookDto>> Handle(BooksQuery request, CancellationToken cancellationToken)
		{
			var datko = new DateTime(1900, 1, 1);

			var querko = (from bl in _dbContext.Localized<Book, BookLocales>(culture: request.LangCode)
						  join cl in _dbContext.Localized<Category, CategoryLocales>(culture: request.LangCode)
							on bl.Entity.CategoryId equals cl.Entity.Id
						  join pub in _dbContext.Set<Publisher>() on bl.Entity.PublisherId equals pub.Id
						  where bl.Entity.PublishDateTime >= datko
						  orderby bl.Entity.Id
						  select new BookDto
						  {
							  Id = bl.Entity.Id,
							  Price = bl.Entity.Price,
							  Title = bl.Translation.Title,
							  NumberOfPages = bl.Entity.NumberOfPages,
							  PublishDateTime = bl.Entity.PublishDateTime,
							  Publisher = new PublisherDto
							  {
								  FirstName = pub.Name
							  },
							  Category = cl.Translation.Name
						  })
						 .Skip((request.Page - 1) * request.PageSize)
						 .Take(request.PageSize);

			var data = await querko.ToListAsync(cancellationToken);

			return data;
		}
	}
}