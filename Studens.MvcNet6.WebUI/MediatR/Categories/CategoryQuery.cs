using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Studens.Commons.Extensions;
using Studens.Data.EntityFrameworkCore;
using Studens.Data.EntityFrameworkCore.Localization;
using Studens.Domain;
using Studens.MvcNet6.WebUI.Data;
using Studens.MvcNet6.WebUI.Domain;

namespace Studens.MvcNet6.WebUI.MediatR.Categories
{
	/// <summary>
	/// Contains translatable entities
	/// </summary>
	public class CategoryTranslationDto
	{
		public string Name { get; set; }

		public string Language { get; set; }

		/// <summary>
		/// Extract this to the interface
		/// </summary>
		public bool HasTranslation => Name.IsNotNullOrEmpty();
	}

	public class CategoryDto : CategoryTranslationDto
	{
		public int Id { get; set; }
	}

	public class CategoryQuery : IRequest<IList<CategoryDto>>
	{
		public int Page { get; set; }
		public int PageSize { get; set; }
		public string LangCode { get; set; }
	}

	public class CategoryHandler : IRequestHandler<CategoryQuery, IList<CategoryDto>>
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly ITranslationManager _translationProcessor;

		public CategoryHandler(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
			// Add reg ext
			_translationProcessor = new EfTranslationManager<ApplicationDbContext>(_dbContext);
		}

		public async Task<IList<CategoryDto>> Handle(CategoryQuery request, CancellationToken cancellationToken)
		{
			Guard.Against.Null(request, nameof(request));

			var categories = await _dbContext.Localized<Category, CategoryLocales>(request.LangCode).Select(cl => new CategoryDto
			{
				Id = cl.Entity.Id,
				Name = cl.Translation.Name,
				Language = cl.Translation.LanguageCode
			})
			.OrderBy(c => c.Id)
			.Skip((request.Page - 1) * request.PageSize)
			.Take(request.PageSize)
			.ToListAsync(cancellationToken);

			// Post process locales without translations
			// Move this to service
			var nonLocalizedEntries = categories
			.Where(e => !e.HasTranslation)
			.Select(e => e.Id)
			.ToList();

			var missingTranslations = await _translationProcessor
			.GetMissingTranslationsAsync<Category, int, CategoryLocales>(nonLocalizedEntries, cancellationToken);

			foreach (var translation in missingTranslations)
			{
				var category = categories.First(c => c.Id == translation.ParentId);
				category.Name = translation.Name;
				category.Language = translation.LanguageCode;
			}

			return categories;
		}
	}
}