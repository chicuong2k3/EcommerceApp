

namespace EcommerceApp.Domain.Models
{
    public class ReviewImage
    {
        public Guid Id { get; set; }
        public Guid ReviewId { get; set; }
        public Guid ImageId { get; set; }
        public Review? Review { get; set; }
        public Image? Image { get; set; }
    }
}
