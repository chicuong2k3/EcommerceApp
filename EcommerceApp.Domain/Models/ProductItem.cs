namespace EcommerceApp.Domain.Models
{
    public class ProductItem
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int ColourId { get; set; }
        public Product Product { get; set; }
        public Colour Colour { get; set; }
    }
}
