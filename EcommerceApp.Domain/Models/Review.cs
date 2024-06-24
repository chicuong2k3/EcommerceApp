namespace EcommerceApp.Domain.Models
{
    public class Review
    {
        public Guid Id { get; set; }
        public required string Comment { get; set; }
        public short Rating { get; set; }
        public required string AppUserId { get; set; }
        public Guid OrderLineId { get; set; }
        public AppUser AppUser { get; set; }
        public OrderLine OrderLine { get; set; }
    }
}
