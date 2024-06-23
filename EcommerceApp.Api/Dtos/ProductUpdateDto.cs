using EcommerceApp.Domain.CusomAttributes;
using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Api.Dtos
{
    public class ProductUpdateDto
    {
        [MaxLength(255)]
        public required string Name { get; set; }
        [MaxLength(1000)]
        public string? Description { get; set; }
        [MaxLength(1024)]
        public string? ThumbUrl { get; set; }

        [Range(0.0, 1000000000.0)]
        public decimal OriginalPrice { get; set; }

        [Range(0.0, 1000000000.0)]
        [LessThan(nameof(OriginalPrice))]
        public decimal SalePrice { get; set; }
    }
}
