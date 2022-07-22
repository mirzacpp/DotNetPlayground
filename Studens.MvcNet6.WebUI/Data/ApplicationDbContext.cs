using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Studens.Data.Localization;
using Studens.Domain.Entities;
using System.Globalization;

namespace Studens.MvcNet6.WebUI.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);
		}

		public IQueryable<QueryLocaleEntry<T, TLocalizedEntity>> Localized<T, TLocalizedEntity>(string culture = null)
		where T : class, IHasLocales<TLocalizedEntity>, IEntity<int>
		where TLocalizedEntity : class, ILocalizedEntity<T, int>
		{
			// Note that we can use CultureInfo.CurrentUICulture for current culture and CultureInfo.CurrentUICulture.Parent as fallback
			culture ??= CultureInfo.CurrentUICulture.Name;

			return from p in this.Set<T>()
				   join c in Set<TLocalizedEntity>() on p.Id equals c.ParentId
				   where c.LanguageCode == culture
				   select new QueryLocaleEntry<T, TLocalizedEntity> { Parent = p, Locale = c };
		}

		public IQueryable<T> QueryLocalized<T, TLocalized>(Func<IQueryable<T>, IQueryable<TLocalized>, IQueryable<T>?> query)
		where T : class, IHasLocales<TLocalized>, IEntity<int>
		where TLocalized : class, ILocalizedEntity<T, int>
		{
			var querko = (from b in Set<T>()
						  join bl in Set<TLocalized>() on b.Id equals bl.ParentId
						  where bl.LanguageCode == "en"
						  select b);

			if (query != null)
			{
				querko = querko.Concat(query(Set<T>(), Set<TLocalized>()));
			}

			return querko;
		}
	}

	public class QueryLocaleEntry<TParent, TLocalizedEntity>
		where TParent : class, IHasLocales<TLocalizedEntity>, IEntity<int>
		where TLocalizedEntity : class, ILocalizedEntity<TParent, int>
	{
		public TParent Parent { get; set; }
		public TLocalizedEntity Locale { get; set; }
	}
}