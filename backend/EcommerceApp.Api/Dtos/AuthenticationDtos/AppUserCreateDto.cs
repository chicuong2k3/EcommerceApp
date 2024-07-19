using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Api.Dtos.AuthenticationDtos
{
    public class AppUserCreateDto
    {

        [MaxLength(100)]
        [Required]
        public string FirstName { get; set; } = default!;

        [MaxLength(100)]
        [Required]
        public string LastName { get; set; } = default!;
        [MaxLength(100)]
        [Required]
        public string UserName { get; set; } = default!;
        [MaxLength(100)]
        [Required]
        public string Password { get; set; } = default!;
        [MaxLength(100)]
        [Required]
        public string Email { get; set; } = default!;
        [MaxLength(100)]
        public string? PhoneNumber { get; set; }

        [MaxLength(1024)]
        public string? AvatarUrl { get; set; }
    }
}
