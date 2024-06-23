using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Api.Dtos
{
    public class CategoryCreateUpdateDto
    {
        [MaxLength(50)]
        public required string Name { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}