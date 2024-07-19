using EcommerceApp.Api.Dtos.CategoryDtos;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Api.Dtos.ProductDtos
{
    public class ProductGetDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public List<ProductItemDto> ProductItems { get; set; } = new();
        public List<CategoryGetDto> Categories { get; set; } = new();

    }

}