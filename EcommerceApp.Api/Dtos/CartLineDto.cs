
namespace EcommerceApp.Api.Dtos
{
    public class CartLineDto
    {
        public ProductCartDto? Product { get; set; }
        public Guid ProductVariationId { get; set; }
        public int Quantity { get; set; }

    }

    
}
