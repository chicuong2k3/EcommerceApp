using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApp.Domain.Models
{
    public class ProductItem
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int QuantityInStock { get; set; }
        public decimal BasePrice { get; set; }
        public Product Product { get; set; } = default!;
        public ICollection<ProductVariation> ProductVariations { get; set; } = new List<ProductVariation>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
    }
}
