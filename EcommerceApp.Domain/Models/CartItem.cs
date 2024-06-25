namespace EcommerceApp.Domain.Models
{
    public class CartItem
    {
        public Guid Id { get; set; }
        public Guid ProductVariationId { get; set; }
        public int Quantity { get; set; }
        public string AppUserId { get; set; }
        public ProductVariation ProductVariation { get; set; }
        public AppUser AppUser { get; set; }
    }
}
