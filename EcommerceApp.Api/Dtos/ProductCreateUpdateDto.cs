using EcommerceApp.Api.CusomAttributes;
using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Api.Dtos
{
    public class ProductCreateUpdateDto
    {
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        [Range(0.0, 1000000000.0)]
        public decimal Price { get; set; }

        [Required]
        [Range(0.0, 1000000000.0)]
        [ProductSalePrice]
        public decimal SalePrice { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}