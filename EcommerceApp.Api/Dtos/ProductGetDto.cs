namespace EcommerceApp.Api.Dtos
{
    public record ProductGetDto(
        int Id,
        string Name,
        string Description,
        decimal Price,
        string PhotoUrl,
        int CategoryId);
}