using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Api.Dtos.CategoryDtos
{
    public class CategoryCreateUpdateDto
    {
        [MaxLength(50)]
        [MinLength(3)]
        public required string Name { get; set; }
    }
}