namespace EcommerceApp.Api.Dtos.CartDtos
{
    public class AddProductToCartDto
    {
        public required Guid ProductId { get; set; }
        public required int ProductVariantNumber { get; set; }
        public int Quantity { get; set; }
    }
}
