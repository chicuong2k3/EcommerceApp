
namespace EcommerceApp.Api.Dtos
{
    public class ProductGetDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public decimal SalePrice { get; set; }
        public string PhotoUrl { get; init; } = string.Empty;
        public int CategoryId { get; init; }

         
    }
}