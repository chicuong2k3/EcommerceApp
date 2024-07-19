using EcommerceApp.Common.Shared;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(Guid id);
        Task<Product> InsertAsync(Product product, IEnumerable<int> categoryIds);
        Task UpdateAsync(Product product, IEnumerable<int> categoryIds);
        Task DeleteAsync(Guid id);

        Task<PagedData<Product>> GetProductsAsync(ProductQueryParameters queryParameters);
    }
}