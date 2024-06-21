using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Domain.Models
{
    public class Colour
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public required string Value { get; set; }
    }
}
