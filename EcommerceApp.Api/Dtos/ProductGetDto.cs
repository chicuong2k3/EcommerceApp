namespace EcommerceApp.Api.Dtos
{
    public record ProductGetDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public decimal Price { get; init; }
        public decimal SalePrice { get; set; }
        public string PhotoUrl { get; init; }
        public int CategoryId { get; init; }
    }
}