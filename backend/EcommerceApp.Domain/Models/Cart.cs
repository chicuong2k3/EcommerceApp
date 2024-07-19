namespace EcommerceApp.Domain.Models
{
    public class Cart
    {
        public string Id { get; set; } = default!;
        public string AppUserId { get; set; } = default!;
        public ProductVariation AppUser { get; set; } = default!;
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
