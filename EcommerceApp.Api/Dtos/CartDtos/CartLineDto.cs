namespace EcommerceApp.Api.Dtos.CartDtos
{
    public class CartLineDto
    {
        public ProductCartDto? Product { get; set; }
        public Guid ProductVariantId{ get; set; }
        public int Quantity { get; set; }

    }


}
