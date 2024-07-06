using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Domain.Models
{
    public class AppUser : IdentityUser
    {

        [MaxLength(100)]
        public required string FirstName { get; set; }

        [MaxLength(100)]
        public required string LastName { get; set; }
        public DateTime RegistrationDate { get; set; }

        [MaxLength(1024)]
        public string? AvatarUrl { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
