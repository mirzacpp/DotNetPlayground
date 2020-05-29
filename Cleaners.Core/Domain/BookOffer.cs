using Cleaners.Core.Interfaces;

namespace Cleaners.Core.Domain
{
    public class BookOffer : IEntity
    {
        public int Id { get; set; }
        public decimal NewPrice { get; set; }
        public string PromotionText { get; set; }
        public int BookId { get; set; }

        public Book Book { get; set; }
    }
}