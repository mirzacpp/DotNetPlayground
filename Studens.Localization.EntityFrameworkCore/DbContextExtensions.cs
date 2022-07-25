using Microsoft.EntityFrameworkCore;
using Studens.Data.Localization;
using Studens.Domain.Entities;
using System.Globalization;

namespace Studens.Localization.EntityFrameworkCore
{
	/// <summary>
	/// Extension methods for <see cref="DbContext"/>
	/// </summary>
	public static class DbContextExtensions
	{
		//public static IQueryable<QueryTranslationPair<TTranslatableEntity, TTranslatableEntityKey, TTranslation>> Localized<TTranslatableEntity, TTranslatableEntityKey, TTranslation>(
		//this DbContext dbContext,
		//string? culture = null)
		//where TTranslatableEntity : class, ITranslatableEntity<TTranslation>, IEntity<TTranslatableEntityKey>
		//where TTranslation : class, IEntityTranslation<TTranslatableEntity, TTranslatableEntityKey>
		//{
		//	// Note that we can use CultureInfo.CurrentUICulture for current culture and CultureInfo.CurrentUICulture.Parent as fallback
		//	culture ??= CultureInfo.CurrentUICulture.Name;

		//	return from parent in dbContext.Set<TTranslatableEntity>()
		//		   join child in dbContext.Set<TTranslation>() on parent.Id equals child.ParentId
		//		   where child.LanguageCode == culture
		//		   select new QueryTranslationPair<TTranslatableEntity, TTranslatableEntityKey, TTranslation>(parent, child);
		//}

		public static IQueryable<QueryTranslationPair<TTranslatableEntity, TTranslation>> Localized<TTranslatableEntity, TTranslation>(
		this DbContext dbContext,
		string? culture = null)
		where TTranslatableEntity : class, ITranslatableEntity<TTranslation>, IEntity<int>
		where TTranslation : class, IEntityTranslation<TTranslatableEntity, int>
		{
			// Note that we can use CultureInfo.CurrentUICulture for current culture and CultureInfo.CurrentUICulture.Parent as fallback
			culture ??= CultureInfo.CurrentUICulture.Name;

			return from entity in dbContext.Set<TTranslatableEntity>()
				   join translation in dbContext.Set<TTranslation>() on entity.Id equals translation.ParentId
				   where translation.LanguageCode == culture
				   select new QueryTranslationPair<TTranslatableEntity, TTranslation>(entity, translation);
		}
	}
}