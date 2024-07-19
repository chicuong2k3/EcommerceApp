namespace EcommerceApp.BlazorWeb.Responses
{
    public class PagedResult<T>
    {
        public List<T> Data { get; set; } = [];
        public PagedInfo Pagination { get; set; } = default!;
    }

    public class PagedInfo
    {
        public int CurrentPage { get; set; }
        public int PerPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
}
