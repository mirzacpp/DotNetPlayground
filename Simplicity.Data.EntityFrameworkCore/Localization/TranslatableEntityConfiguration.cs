using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simplicity.Domain.Entities;

namespace Microsoft.EntityFrameworkCore
{
	/// <summary>
	/// Predefines base configuration for <typeparamref name="TTranslation"/> using <see cref="int"/> as an PK type. Associated with <typeparamref name="TTranslatableEntity"/>.
	/// </summary>
	/// <typeparam name="TTranslation">Entity translation type</typeparam>
	/// <typeparam name="TTranslatableEntity">Translatable entity type</typeparam>
	public abstract class TranslatableEntityConfiguration<TTranslation, TTranslatableEntity> : TranslatableEntityConfiguration<TTranslation, TTranslatableEntity, int>
		where TTranslation : class, IEntityTranslation<TTranslatableEntity, int>
		where TTranslatableEntity : class, ITranslatableEntity<TTranslation>
	{
	}

	/// <summary>
	/// Defines base configuration for <typeparamref name="TTranslation"/> associated with <typeparamref name="TTranslatableEntity"/>.
	/// </summary>
	/// <typeparam name="TTranslation">Entity translation type</typeparam>
	/// <typeparam name="TTranslatableEntity">Translatable entity type</typeparam>
	/// <typeparam name="TTranslatableEntityKey">Translatable entity PK type</typeparam>
	public abstract class TranslatableEntityConfiguration<TTranslation, TTranslatableEntity, TTranslatableEntityKey> : IEntityTypeConfiguration<TTranslation>
		where TTranslation : class, IEntityTranslation<TTranslatableEntity, TTranslatableEntityKey>
		where TTranslatableEntity : class, ITranslatableEntity<TTranslation>
	{
		public virtual void Configure(EntityTypeBuilder<TTranslation> builder)
		{
			builder.ToTable(typeof(TTranslation).Name);
			// We should be enough with 20 chars.
			builder.Property(p => p.LanguageCode).HasMaxLength(20);
			builder.HasIndex(p => new { p.LanguageCode, p.ParentId }).IsUnique();
			builder.HasOne(p => p.Parent).WithMany(c => c.Translations).HasForeignKey(p => p.ParentId);
		}
	}
}