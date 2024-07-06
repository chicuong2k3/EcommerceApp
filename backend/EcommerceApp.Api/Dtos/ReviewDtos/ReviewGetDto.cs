using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Api.Dtos.ReviewDtos
{
    public class ReviewGetDto
    {
        public Guid Id { get; set; }
        public string Comment { get; set; } = string.Empty;
        public short Rating { get; set; }
        public string AppUserId { get; set; } = string.Empty;
        public Guid OrderLineId { get; set; }
    }
}
