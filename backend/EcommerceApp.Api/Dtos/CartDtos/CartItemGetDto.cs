namespace EcommerceApp.Api.Dtos.CartDtos
{
    public class CartItemGetDto
    {
        public Guid CartItemId { get; set; }
        public Guid ProductItemId { get; set; }
        public int Quantity { get; set; }

    }


}
