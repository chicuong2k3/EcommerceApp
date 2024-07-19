using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Domain.Models
{
    public class OrderStatus
    {
        public int Id { get; set; }
        public string Value { get; set; } = default!;
    }
}
