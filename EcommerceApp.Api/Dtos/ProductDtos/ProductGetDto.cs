using EcommerceApp.Api.Dtos.CategoryDtos;

namespace EcommerceApp.Api.Dtos.ProductDtos
{
    public class ProductGetDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string ThumbUrl { get; init; } = string.Empty;
        public decimal OriginalPrice { get; set; }
        public decimal SalePrice { get; set; }
        public List<CategoryGetDto> Categories { get; set; }
        public List<ColourGetDto> Colours { get; set; }

    }

}