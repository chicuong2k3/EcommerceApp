namespace EcommerceApp.Api.Dtos.AuthenticationDtos
{
    public class TokenDto
    {
        public required string AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? ExpiryTime { get; set; }
    }
}
