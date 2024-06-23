namespace EcommerceApp.Domain.Shared
{
    public class PagingData<T> where T : class
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }

        public List<T> Items { get; set; } = new List<T>();
        public PagingData(List<T> items, int pageNumber, int pageSize, int totalItems)
        {
            CurrentPage = pageNumber;
            PageSize = pageSize;
            TotalItems = totalItems;
            TotalPages = (int)Math.Ceiling(TotalItems / (double)PageSize);
            Items.AddRange(items);
        }
    }
}
