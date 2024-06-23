using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Domain.Models
{
    public class Category
    {

        public int Id { get; set; }
        [MaxLength(50)]
        public required string Name { get; set; }
        public int? ParentCategoryId { get; set; }
        public Category? ParentCategory { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}