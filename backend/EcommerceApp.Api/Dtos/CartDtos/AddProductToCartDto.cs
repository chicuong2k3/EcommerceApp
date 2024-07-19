using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Api.Dtos.CartDtos
{
    public class AddProductToCartDto
    {
        [Required]
        public Guid ProductItemId { get; set; } = default!;
        [Range(0, 10000)]
        public int Quantity { get; set; }
    }
}
