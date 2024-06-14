using EcommerceApp.Domain.CusomAttributes;

namespace EcommerceApp.Domain.Shared
{
    public class ProductQueryParameters : QueryParameters
    {

        public int? CategoryId { get; set; }

        [LessThanOrEqual(nameof(MaxPrice))]
        public decimal MinPrice { get; set; } = 0;
        public decimal MaxPrice { get; set; } = int.MaxValue;
        public string? Keyword { get; set; }
    }
}
