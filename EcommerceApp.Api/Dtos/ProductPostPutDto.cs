using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Api.Dtos
{
    public class ProductPostPutDto
    {
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0, 1000000000)]
        public decimal Price { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}