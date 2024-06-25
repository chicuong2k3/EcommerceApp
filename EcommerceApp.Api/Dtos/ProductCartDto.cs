namespace EcommerceApp.Api.Dtos
{
    public class ProductCartDto
    {
        public string Name { get; init; } = string.Empty;
        public string ThumbUrl { get; init; } = string.Empty;
        public decimal OriginalPrice { get; set; }
        public decimal SalePrice { get; set; }
        public string Colour { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;

    }
}
