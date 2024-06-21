using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApp.Domain.Models
{
    public class ProductImage
    {
        public Guid Id { get; set; }
        public Guid ProductItemId { get; set; }
        public Guid ImageId { get; set; }
        public ProductItem ProductItem { get; set; }
        public Image Image { get; set; }
    }
}
