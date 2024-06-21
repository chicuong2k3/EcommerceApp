using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Domain.Models
{
    public class Address
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public required string City { get; set; }
        [MaxLength(100)]
        public required string District { get; set; }
        [MaxLength(100)]
        public required string Town { get; set; }
    }
}
