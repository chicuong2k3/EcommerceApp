

namespace EcommerceApp.Domain.Models
{
    public class ProductVariation
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Value { get; set; } = default!;
        public ICollection<ProductItem> ProductItems { get; set; } = new List<ProductItem>();
    }
}
