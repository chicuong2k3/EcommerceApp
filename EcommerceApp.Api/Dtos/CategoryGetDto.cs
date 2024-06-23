

namespace EcommerceApp.Api.Dtos
{
    public class CategoryGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ParentCategoryDto? ParentCategory { get; set; }
    }
}