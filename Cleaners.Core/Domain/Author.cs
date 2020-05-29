using Cleaners.Core.Interfaces;
using System.Collections.Generic;

namespace Cleaners.Core.Domain
{
    public class Author : IEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<AuthorBook> Books { get; set; }
    }
}