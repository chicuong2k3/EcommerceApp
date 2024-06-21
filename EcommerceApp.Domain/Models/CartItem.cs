namespace EcommerceApp.Domain.Models
{
    public class CartItem
    {
        public Guid Id { get; set; }
        public Guid ProductItemId { get; set; }
        public int Quantity { get; set; }
        public Guid CartId { get; set; }
        public ProductItem ProductItem { get; set; }
        public Cart Cart { get; set; }
    }
}
