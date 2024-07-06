using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Api.Dtos.OrderDtos
{
    public class CreateOrderDto
    {
        public int ShippingMethodId { get; set; }
        public required string AppUserId { get; set; }
        [MaxLength(250)]
        public required string Address { get; set; }
        [MaxLength(20)]
        public required string PhoneNumber { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal Discount { get; set; }
        public int PaymentMethodId { get; set; }
        public List<OrderDetail>? OrderDetails { get; set; }
    }

    public class OrderDetail
    {
        public Guid ProductId { get; set; }
        public int VariantNumber { get; set; }
        public int Quantity { get; set; }

    }
}
