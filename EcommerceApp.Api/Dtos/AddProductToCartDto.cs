namespace EcommerceApp.Api.Dtos
{
    public class AddProductToCartDto
    {
        public required string AppUserId { get; set; }
        public required Guid ProductVariationId { get; set; }
        public int Quantity { get; set; }
    }
}
