using Microsoft.EntityFrameworkCore;
using Studens.Domain.Entities;
using System.Globalization;

namespace Studens.Data.EntityFrameworkCore
{
	/// <summary>
	/// Extension methods for <see cref="DbContext"/>
	/// </summary>
	public static class DbContextExtensions
	{
		/// <summary>
		/// Returns localized <see cref="IQueryable{QueryTranslationPair}"/> for entity <typeparamref name="TTranslatableEntity"/> and translation <typeparamref name="TTranslation"/>.
		/// </summary>
		/// <typeparam name="TTranslatableEntity">Translatable entity type</typeparam>
		/// <typeparam name="TTranslation">Entity translation type</typeparam>
		/// <param name="dbContext">Current DbContext instance</param>
		/// <param name="culture">Culture used for filtering. If null, uses	<see cref="CultureInfo.CurrentUICulture"/> to obtain current culture.</param>
		/// <returns></returns>
		public static IQueryable<QueryTranslationPair<TTranslatableEntity, int, TTranslation>> Localized<TTranslatableEntity, TTranslation>(
		this DbContext dbContext,
		string? culture = null)
		where TTranslatableEntity : class, ITranslatableEntity<TTranslation>, IEntity<int>
		where TTranslation : class, IEntityTranslation<TTranslatableEntity, int>
		{
			// Note that we can use CultureInfo.CurrentUICulture for current culture and CultureInfo.CurrentUICulture.Parent as fallback
			culture ??= CultureInfo.CurrentUICulture.Name;

			return from entity in dbContext.Set<TTranslatableEntity>()
				   join translation in dbContext.Set<TTranslation>() on entity.Id equals translation.ParentId
				   where translation.LanguageCode.Equals(culture)
				   // NOTE that we do not use ctor for QueryTranslationPair since they cannot be translated to SQL.
				   select new QueryTranslationPair<TTranslatableEntity, TTranslation> { Entity = entity, Translation = translation };
		}

		/// <summary>
		/// Returns localized <see cref="IQueryable{QueryTranslationPair}"/> for entity <typeparamref name="TTranslatableEntity"/> and translation <typeparamref name="TTranslation"/>.
		/// </summary>
		/// <typeparam name="TTranslatableEntity">Translatable entity type</typeparam>
		/// <typeparam name="TTranslation">Entity translation type</typeparam>
		/// <param name="dbContext">Current DbContext instance</param>
		/// <param name="culture">Culture used for filtering. If null, uses	<see cref="CultureInfo.CurrentUICulture"/> to obtain current culture.</param>
		/// <returns></returns>
		public static IQueryable<QueryTranslationPair<TTranslatableEntity, TTranslatableEntityKey, TTranslation>> Localized<TTranslatableEntity, TTranslatableEntityKey, TTranslation>(
		this DbContext dbContext,
		string? culture = null)
		where TTranslatableEntity : class, ITranslatableEntity<TTranslation>, IEntity<TTranslatableEntityKey>
		where TTranslation : class, IEntityTranslation<TTranslatableEntity, TTranslatableEntityKey>
		{
			// Note that we can use CultureInfo.CurrentUICulture for current culture and CultureInfo.CurrentUICulture.Parent as fallback
			culture ??= CultureInfo.CurrentUICulture.Name;

			return from entity in dbContext.Set<TTranslatableEntity>()
				   join translation in dbContext.Set<TTranslation>() on entity.Id equals translation.ParentId
				   where translation.LanguageCode.Equals(culture)
				   // NOTE that we do not use ctor for QueryTranslationPair since they cannot be translated to SQL.
				   select new QueryTranslationPair<TTranslatableEntity, TTranslatableEntityKey, TTranslation> { Entity = entity, Translation = translation };
		}
	}
}