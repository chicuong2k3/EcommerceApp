namespace EcommerceApp.Api.Dtos.CartDtos
{
    public class CartItemGetDto
    {
        public Guid CartItemId { get; set; }
        public Guid ProductId { get; set; }
        public int VariantNumber { get; set; }
        public int Quantity { get; set; }

    }


}
