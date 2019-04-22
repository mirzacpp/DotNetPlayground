using System;

namespace Cleaners.Core.Interfaces
{
    public interface IAuditableEntity
    {
        DateTime CreationDateUtc { get; set; }
        DateTime? LastUpdateDateUtc { get; set; }
    }
}