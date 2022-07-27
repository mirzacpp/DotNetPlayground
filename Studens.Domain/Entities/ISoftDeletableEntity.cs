namespace Studens.Domain.Entities
{
	/// <summary>
	/// Marks entity as soft deleteable.
	///	Soft deleted entities remain in storage and by default are not returned in queries.
	/// </summary>
	public interface ISoftDeletableEntity
	{
		bool IsDeleted { get; set; }
	}
}