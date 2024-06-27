namespace EcommerceApp.Domain.Models
{
    public class Cart
    {
        public Guid Id { get; set; }
        public required string AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
