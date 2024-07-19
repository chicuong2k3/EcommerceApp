using EcommerceApp.Common.CusomAttributes;
using EcommerceApp.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Api.Dtos.ProductDtos
{
    public class ProductCreateUpdateDto
    {
        [MaxLength(255)]
        [MinLength(10)]
        [Required]
        public string Name { get; set; } = default!;
        [MaxLength(1000)]
        public string? Description { get; set; }
        public List<ProductItemDto> ProductItems { get; set; } = new();
        [Required]
        public IEnumerable<int> CategoryIds { get; set; } = new List<int>();
    }

}