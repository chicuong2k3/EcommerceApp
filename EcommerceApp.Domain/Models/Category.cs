using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Domain.Models
{
    public class Category
    {

        public int Id { get; set; }
        [MaxLength(100)]
        public required string Name { get; set; }
        [MaxLength(100)]
        public required string Slug { get; set; }
    }
}