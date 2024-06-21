using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Domain.Models
{
    public class AppUser
    {
        public Guid Id { get; set; }

        [MaxLength(100)]
        public required string FirstName { get; set; }

        [MaxLength(100)]
        public required string LastName { get; set; }

        [MaxLength(100)]
        public required string Password { get; set; }

        [MaxLength(20)]
        public required string PhoneNumber { get; set; }
        public bool PhoneVerified { get; set; } = false;

        [MaxLength(50)]
        public required string Email { get; set; }
        public bool EmailVerified { get; set; } = false;
        public DateTime RegistrationDate { get; set; }

        [MaxLength(1024)]
        public string? AvatarUrl { get; set; }
    }
}
