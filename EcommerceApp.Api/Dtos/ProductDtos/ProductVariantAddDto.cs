namespace EcommerceApp.Api.Dtos.ProductDtos
{
    public class ProductVariantAddDto
    {
        public Guid ProductId { get; set; }
        public int ColourId { get; set; }
        public int SizeId { get; set; }
        public int QuantityInStock { get; set; }
    }

}
