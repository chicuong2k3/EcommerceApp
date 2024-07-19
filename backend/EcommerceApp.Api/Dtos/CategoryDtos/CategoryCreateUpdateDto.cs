using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Api.Dtos.CategoryDtos
{
    public class CategoryCreateUpdateDto
    {
        [MaxLength(50)]
        [MinLength(3)]
        [Required]
        public string Name { get; set; } = default!;
    }
}