namespace EcommerceApp.Api.Dtos.CategoryDtos
{
    public class CategoryGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
    }
}