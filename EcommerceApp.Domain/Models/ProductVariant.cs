namespace EcommerceApp.Domain.Models
{
    public class ProductVariant
    {
        public int VariantNumber { get; set; }
        public Guid ProductId { get; set; }
        public int ColourId { get; set; }
        public int SizeId { get; set; }
        public int QuantityInStock { get; set; }
        public Colour? Colour { get; set; }
        public Size? Size { get; set; }
        public Product? Product { get; set; }
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<ProductImage>? ProductImages { get; set; }
        public ICollection<OrderLine>? OrderLines { get; set; }
    }
}
