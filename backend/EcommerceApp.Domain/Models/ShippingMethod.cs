using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Domain.Models
{
    public class ShippingMethod
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public required string ShippingProvider { get; set; }
    }
}
