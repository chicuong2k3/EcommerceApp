namespace EcommerceApp.Api.Dtos.CartDtos
{
    public class AddProductToCartDto
    {
        public required string AppUserId { get; set; }
        public required Guid ProductVariantId { get; set; }
        public int Quantity { get; set; }
    }
}
