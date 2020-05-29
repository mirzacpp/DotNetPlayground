namespace Cleaners.Core.Interfaces
{
    /// <summary>
    /// Enables soft delete for entities
    /// </summary>
    public interface ISoftDeletableEntity
    {
        bool IsDeleted { get; set; }
    }
}