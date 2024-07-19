namespace EcommerceApp.Common.Shared
{
    public class PagedData<T> where T : class
    {
        public Pagination Pagination { get; set; }

        public List<T> Items { get; set; } = new List<T>();
        public PagedData(List<T> items, int pageNumber, int pageSize, int totalItems)
        {
            Pagination = new Pagination();
            Pagination.CurrentPage = pageNumber;
            Pagination.PerPage = pageSize;
            Pagination.TotalItems = totalItems;
            Pagination.TotalPages = (int)Math.Ceiling(Pagination.TotalItems / (double)Pagination.PerPage);
            Items.AddRange(items);
        }
    }
}
