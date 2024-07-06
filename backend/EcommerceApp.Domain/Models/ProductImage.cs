using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApp.Domain.Models
{
    public class ProductImage
    {
        public Guid Id { get; set; }
        public int VariantNumber { get; set; }
        public Guid ProductId { get; set; }
        public Guid ImageId { get; set; }
        public ProductVariant? ProductVariant { get; set; }
        public Image? Image { get; set; }
    }
}
