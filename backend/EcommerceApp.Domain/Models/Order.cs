using EcommerceApp.Common.Constants;
using EcommerceApp.Common.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApp.Domain.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        //public int ShippingMethodId { get; set; }
        public string AppUserId { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public DateTime OrderDate { get; set; }
        //public int PaymentMethodId { get; set; }
        public int OrderStatusId { get; set; }
        //public ShippingMethod ShippingMethod { get; set; } = default!;
        public ProductVariation AppUser { get; set; } = default!;
        //public PaymentMethod PaymentMethod { get; set; } = default!;
        public OrderStatus OrderStatus { get; set; } = default!;
        public ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
    }
}
