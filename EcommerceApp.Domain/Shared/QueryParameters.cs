using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Domain.Shared
{
    public abstract class QueryParameters
    {
        [Range(1, 50)]
        public int PageSize { get; set; } = 5;
        public int PageNumber { get; set; } = 1;
        public string? OrderBy { get; set; }

    }
}
