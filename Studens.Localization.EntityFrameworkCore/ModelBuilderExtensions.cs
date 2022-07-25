using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Studens.Data.Localization;

namespace Microsoft.EntityFrameworkCore
{
	/// <summary>
	/// Loclization extension methods for <see cref="ModelBuilder"/>
	/// </summary>
	public static class ModelBuilderExtensions
	{
		public static ModelBuilder ConfigureTranslatableEntities(this ModelBuilder builder)
		{
			var translatableTypes = builder.Model.GetEntityTypes()
				.Select(t => t.ClrType)
				.Where(t => typeof(IEntityTranslation).IsAssignableFrom(t));

			//foreach (var type in translatableTypes)
			//{
			//	builder.Entity(type).HasIndex((IEntityTranslation)b => b.Lang);
			//}

			return builder;
		}
	}

	public abstract class TranslatableEntityConfiguration<TTranslation, TEntity> : IEntityTypeConfiguration<TTranslation>
		where TTranslation : class, IEntityTranslation<TEntity>
		where TEntity : class, ITranslatableEntity<TTranslation>
	{
		public virtual void Configure(EntityTypeBuilder<TTranslation> builder)
		{
			//3-chars should be enough for 2/3digit iso codes.
			builder.Property(p => p.LanguageCode).HasMaxLength(3);
			builder.HasIndex(p => new { p.LanguageCode, p.ParentId }).IsUnique();
			//builder.HasOne(p => p.Parent).WithMany(c => c.Translations).HasForeignKey(p => p.ParentId);
		}
	}
}