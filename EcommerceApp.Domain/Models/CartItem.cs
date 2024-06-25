namespace EcommerceApp.Domain.Models
{
    public class CartItem
    {
        public Guid Id { get; set; }
        public Guid ProductVariantId { get; set; }
        public int Quantity { get; set; }
        public string AppUserId { get; set; }
        public ProductVariant ProductVariant { get; set; }
        public AppUser AppUser { get; set; }
    }
}
