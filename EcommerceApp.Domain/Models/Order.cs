using EcommerceApp.Domain.Constants;
using EcommerceApp.Domain.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApp.Domain.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public int ShippingMethodId { get; set; }
        public required string AppUserId { get; set; }
        public int AddressId { get; set; }
        [MaxLength(20)]
        public required string PhoneNumber { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingFee { get; set; }
        public DateTime OrderDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Discount { get; set; } = 0;
        public int PaymentMethodId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }
        public ShippingMethod? ShippingMethod { get; set; }
        public AppUser? AppUser { get; set; }
        public Address? Address { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        [MaxLength(50)]
        public string OrderStatus { get; set; } = OrderStatusConstant.Pending;
    }
}
