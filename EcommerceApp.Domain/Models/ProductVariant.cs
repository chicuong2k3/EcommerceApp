namespace EcommerceApp.Domain.Models
{
    public class ProductVariant
    {
        public Guid Id { get; set; }
        public Guid ProductItemId { get; set; }
        public int SizeId { get; set; }
        public int QuantityInStock { get; set; }
        public Size Size { get; set; }
        public ProductItem ProductItem { get; set; }
    }
}
