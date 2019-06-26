using System;

namespace Cleaners.Core.Interfaces
{
    /// <summary>
    /// Interface for entitiy auditing
    /// </summary>
    public interface IAuditableEntity
    {
        DateTime CreationDateUtc { get; set; }
        DateTime? LastUpdateDateUtc { get; set; }
    }
}