namespace EcommerceApp.Api.Dtos.ReviewDtos
{
    public class ReviewUpdateDto
    {
        public string Comment { get; set; } = string.Empty;
        public short Rating { get; set; }
    }
}
