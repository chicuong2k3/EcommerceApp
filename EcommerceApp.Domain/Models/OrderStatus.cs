using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Domain.Models
{
    public class OrderStatus
    {
        public int Id { get; set; }
        [MaxLength(20)]
        public required string Value { get; set; }
    }
}
