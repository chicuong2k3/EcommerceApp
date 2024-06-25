namespace EcommerceApp.Api.Dtos.SharedDtos
{
    public class PaginationDto
    {
        public int CurrentPage { get; set; }
        public int PerPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
}
