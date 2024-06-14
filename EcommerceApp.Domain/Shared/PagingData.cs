namespace EcommerceApp.Domain.Shared
{
    public class PagingData<T> where T : class
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }

        public List<T> Items { get; set; } = new List<T>();
        public PagingData(List<T> items, int pageNumber, int pageSize)
        {
            CurrentPage = pageNumber;
            PageSize = pageSize;
            TotalItems = items.Count;
            TotalPages = (int)Math.Ceiling(items.Count / (double)pageSize);
            Items.AddRange(items);
        }
    }
}
