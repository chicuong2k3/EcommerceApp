using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Api.Dtos
{
    public class AppUserCreateDto 
    {

        [MaxLength(100)]
        public required string FirstName { get; set; }

        [MaxLength(100)]
        public required string LastName { get; set; }
        [MaxLength(100)]
        public required string UserName { get; set; }
        [MaxLength(100)]
        public required string Password { get; set; }
        [MaxLength(100)]
        public required string Email { get; set; }
        [MaxLength(100)]
        public string? PhoneNumber { get; set; }

        [MaxLength(1024)]
        public string? AvatarUrl { get; set; }
    }
}
