using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Domain.Models
{
    public class Review
    {
        public Guid Id { get; set; }
        public string Comment { get; set; } = default!;
        public short Rating { get; set; }
        public string AppUserId { get; set; } = default!;
        public Guid OrderLineId { get; set; }
        public ProductVariation AppUser { get; set; } = default!;
        public OrderLine OrderLine { get; set; } = default!;
    }
}
