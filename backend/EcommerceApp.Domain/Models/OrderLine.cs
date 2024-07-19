using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApp.Domain.Models
{
    public class OrderLine
    {
        public Guid Id { get; set; }
        public Guid ProductItemId { get; set; }
        public Guid OrderId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public ProductItem ProductItem { get; set; } = default!;
        public Order Order { get; set; } = default!;
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
