namespace Cleaners.Core.Interfaces
{
    public interface ISoftDeletableEntity
    {
        bool IsDeleted { get; set; }
    }
}