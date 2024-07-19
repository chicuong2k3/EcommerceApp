using EcommerceApp.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Api.Dtos.ProductDtos
{
    public class ProductItemDto
    {
        public int QuantityInStock { get; set; }
        public decimal BasePrice { get; set; }
        public List<ProductVariationDto> ProductVariations { get; set; } = new();
    }
}
