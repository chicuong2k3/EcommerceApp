namespace EcommerceApp.Domain.Models
{
    public class CartItem
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int VariantNumber { get; set; }
        public int Quantity { get; set; }
        public Guid CartId { get; set; }
        public ProductVariant? ProductVariant { get; set; }
        public Cart? Cart { get; set; }
    }
}
