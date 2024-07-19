namespace EcommerceApp.Api.Dtos.SharedDtos
{
    public class PagedDataDto<T>
    {
        public PaginationDto Pagination { get; set; } = default!;
        public List<T> Items { get; set; } = new List<T>();
    }

}
