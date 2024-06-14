using EcommerceApp.Api.CusomAttributes;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Api.Dtos
{
    public class ProductCreateUpdateDto
    {
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Range(0.0, 1000000000.0)]
        public decimal Price { get; set; }

        [Range(0.0, 1000000000.0)]
        [LessThan(nameof(Price))]
        public decimal SalePrice { get; set; }

        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }
    }
}