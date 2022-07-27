using Microsoft.EntityFrameworkCore;
using Studens.Domain;
using Studens.Domain.Entities;
using System.Globalization;

namespace Studens.Data.EntityFrameworkCore.Localization
{
	public class EfTranslationProcessor<TDbContext> : ITranslationManager
		where TDbContext : DbContext
	{
		private readonly TDbContext _dbContext;

		protected const int MaxCultureFallbackDepth = 5;

		public EfTranslationProcessor(TDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<IList<TTranslation>> GetMissingTranslationsAsync<TTranslatableEntity, TTranslatableEntityKey, TTranslation>(
		IList<TTranslatableEntityKey> entityIds,
		CancellationToken cancellationToken = default)
		where TTranslation : class, IEntityTranslation<TTranslatableEntity, TTranslatableEntityKey>
		where TTranslatableEntity : class, ITranslatableEntity<TTranslation>
		{
			if (!entityIds.Any())
			{
				return new List<TTranslation>();
			}

			// Add performance note here

			var culture = CultureInfo.CurrentUICulture.Parent;
			var defaultCulture = new CultureInfo("bs");
			var translations = new List<TTranslation>();

			// Enable this check to be configurable
			// First we will check with current culture parent culture, ie. sr-Latn for sr-Latn-BA (Serbian, Latin, Bosnia and Herzegovina)
			await GetTranslationsRecursive<TTranslatableEntity, TTranslatableEntityKey, TTranslation>(
			translations,
			entityIds,
			culture,
			currentDepth: 0,
			cancellationToken);

			// Last we can try is to get default language translations that should always be present.
			// Add resolver for default language
			// TODO: should we check parent cultures for default culture ?
			if (entityIds.Count != translations.Count)
			{
				entityIds = entityIds.Where(id => translations.Any(t => !t.ParentId.Equals(id))).ToList();
				translations.AddRange(await GetTranslations<TTranslatableEntity, TTranslatableEntityKey, TTranslation>(entityIds, defaultCulture.Name, cancellationToken));
			}

			return translations;
		}

		private async Task<IEnumerable<TTranslation>> GetTranslationsRecursive<TTranslatableEntity, TTranslatableEntityKey, TTranslation>(
		List<TTranslation> translations,
		IList<TTranslatableEntityKey> entityIds,
		CultureInfo culture,
		int currentDepth,
		CancellationToken cancellationToken)
		where TTranslation : class, IEntityTranslation<TTranslatableEntity, TTranslatableEntityKey>
		where TTranslatableEntity : class, ITranslatableEntity<TTranslation>
		{
			if (culture is null || string.IsNullOrEmpty(culture.Name) || entityIds.Count == translations.Count || currentDepth > MaxCultureFallbackDepth)
			{
				return translations;
			}

			translations.AddRange(await GetTranslations<TTranslatableEntity, TTranslatableEntityKey, TTranslation>(entityIds, culture.Name, cancellationToken));

			// Remove found parent ids
			entityIds = translations.Where(t => !entityIds.Contains(t.ParentId)).Select(t => t.ParentId).ToList();

			return await GetTranslationsRecursive<TTranslatableEntity, TTranslatableEntityKey, TTranslation>(translations, entityIds, culture.Parent, currentDepth + 1, cancellationToken);
		}

		private async Task<IEnumerable<TTranslation>> GetTranslations<TTranslatableEntity, TTranslatableEntityKey, TTranslation>(
		IEnumerable<TTranslatableEntityKey> entityIds,
		string culture,
		CancellationToken cancellationToken)
		where TTranslation : class, IEntityTranslation<TTranslatableEntity, TTranslatableEntityKey>
		where TTranslatableEntity : class, ITranslatableEntity<TTranslation>
		{
			return await _dbContext.Set<TTranslation>()
			.Where(t => entityIds.Contains(t.ParentId))
			.Where(t => t.LanguageCode.Equals(culture))
			.ToListAsync(cancellationToken);
		}
	}
}