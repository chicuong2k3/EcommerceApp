namespace EcommerceApp.Domain.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}