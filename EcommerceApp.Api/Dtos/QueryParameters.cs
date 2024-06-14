using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Api.Dtos
{
    public abstract class QueryParameters 
    {
        [Range(1, 50)]
        public int PageSize { get; set; } = 5;
        public int PageNumber { get; set; } = 1;
    }
}
