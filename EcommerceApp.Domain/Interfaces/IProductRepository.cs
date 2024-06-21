using EcommerceApp.Domain.Models;
using EcommerceApp.Domain.Shared;

namespace EcommerceApp.Domain.Interfaces
{
    public interface IProductRepository
    {
        //Task<bool> AddToCategoryAsync(int categoryId, int productId);

        //Task<bool> RemoveFromCategoryAsync(int categoryId, int productId);
        Task<Product?> GetByIdAsync(Guid id);
        Task<Product> InsertAsync(Product product);
        Task<bool> UpdateAsync(Product product);
        Task<bool> DeleteAsync(Guid id);

        Task<PagingData<Product>> GetProductsAsync(ProductQueryParameters queryParameters);

        Task<List<Product>> GetProductsByIdsAsync(IEnumerable<Guid> ids);
    }
}