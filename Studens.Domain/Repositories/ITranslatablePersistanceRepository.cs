using Studens.Domain.Entities;

namespace Studens.Domain.Repositories
{
	public interface ITranslatablePersistanceRepository<TTranslatableEntity, TTranslation>
	where TTranslatableEntity : class, ITranslatableEntity<TTranslation>
	where TTranslation: class, IEntityTranslation<TTranslatableEntity>
	{
		
	}
}