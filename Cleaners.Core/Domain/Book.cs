using Cleaners.Core.Interfaces;
using System.Collections.Generic;

namespace Cleaners.Core.Domain
{
    public class Book : IEntity, ISoftDeletableEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Pages { get; set; }
        public bool IsDeleted { get; set; }

        public BookOffer Offer { get; set; }
        public ICollection<AuthorBook> Authors { get; set; }
    }
}