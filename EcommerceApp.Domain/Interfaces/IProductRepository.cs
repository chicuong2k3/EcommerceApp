using EcommerceApp.Domain.Models;
using EcommerceApp.Domain.Shared;

namespace EcommerceApp.Domain.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        //Task<bool> AddToCategoryAsync(int categoryId, int productId);

        //Task<bool> RemoveFromCategoryAsync(int categoryId, int productId);

        Task<PagingData<Product>> GetProductsAsync(ProductQueryParameters queryParameters);

        Task<List<Product>> GetProductsByIdsAsync(IEnumerable<int> ids);
    }
}