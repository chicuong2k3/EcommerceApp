using EcommerceApp.Common.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApp.Api.Dtos.OrderDtos
{
    public class OrderGetDto
    {
        public Guid Id { get; set; }
        public int ShippingMethodId { get; set; }
        public string AppUserId { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public decimal ShippingFee { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Discount { get; set; }
        public int PaymentMethodId { get; set; }
        public decimal Total { get; set; }
        public string OrderStatus { get; set; } = string.Empty;
    }
}
