
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Api.Dtos
{
    public class ProductGetDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string ThumbUrl { get; init; } = string.Empty;
        public decimal OriginalPrice { get; set; }
        public decimal SalePrice { get; set; }
        public List<Colour> Colours { get; set; } = new List<Colour>();
        public List<Size> Sizes { get; set; } = new List<Size>();
        public int CategoryId { get; init; }

         
    }
}