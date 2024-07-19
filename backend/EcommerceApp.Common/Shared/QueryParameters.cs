using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Common.Shared
{
    public abstract class QueryParameters
    {
        [Range(1, 50)]
        public int Limit { get; set; } = 5;
        public int Page { get; set; } = 1;
        public string? SortBy { get; set; }

    }
}
