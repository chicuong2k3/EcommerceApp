
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Api.Dtos
{
    public class ProductGetDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string ThumbUrl { get; init; } = string.Empty;
        public decimal OriginalPrice { get; set; }
        public decimal SalePrice { get; set; }
        public List<string> Categories { get; set; } = new List<string>();
        public Dictionary<string, List<ProductVariationDto>> OptionsForColour { get; set; } = new Dictionary<string, List<ProductVariationDto>>();

    }

}