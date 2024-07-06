using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Domain.Models
{
    public class Size
    {
        public int Id { get; set; }
        [MaxLength(10)]
        public required string Value { get; set; }
        public ICollection<ProductVariant> ProductVariant { get; set; } = new List<ProductVariant>();
    }
}
