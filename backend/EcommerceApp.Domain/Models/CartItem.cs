namespace EcommerceApp.Domain.Models
{
    public class CartItem
    {
        public string Id { get; set; } = default!;
        public Guid ProductItemId { get; set; }
        public int Quantity { get; set; }
        public string CartId { get; set; } = default!;
        public ProductItem ProductItem { get; set; } = default!;
        public Cart Cart { get; set; } = default!;
    }
}
