using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApp.Domain.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public ICollection<ProductItem> ProductItems { get; set; } = new List<ProductItem>();
        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}