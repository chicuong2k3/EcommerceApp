using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Api.Dtos
{
    public class CategoryPostPutDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}