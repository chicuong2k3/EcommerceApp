using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApp.Domain.Models
{
    public class OrderLine
    {
        public Guid Id { get; set; }
        public int VariantNumber { get; set; }
        public Guid ProductId { get; set; }
        public Guid OrderId { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public ProductVariant? ProductVariant { get; set; }
        public Order? Order { get; set; }
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
