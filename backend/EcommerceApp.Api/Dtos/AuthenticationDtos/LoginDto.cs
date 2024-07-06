using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Api.Dtos.AuthenticationDtos
{
    public class LoginDto
    {
        [MaxLength(100)]
        public required string UserName { get; set; }
        [MaxLength(100)]
        public required string Password { get; set; }
    }
}
