namespace EcommerceApp.Domain.Models
{
    public class Cart
    {
        public Guid Id { get; set; }
        public Guid AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
