using System.ComponentModel.Design.Serialization;

namespace EcommerceApp.Api.Dtos
{
    public record CategoryGetDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
    }
}