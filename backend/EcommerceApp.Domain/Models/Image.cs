using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Domain.Models
{
    public class Image
    {
        public Guid Id { get; set; }
        [MaxLength(1024)]
        public required string Href { get; set; }
        [MaxLength(255)]
        public required string Title { get; set; }
    }
}
