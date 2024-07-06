using EcommerceApp.Domain.Models;
using EcommerceApp.Domain.Shared;

namespace EcommerceApp.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(Guid id);
        Task<Product?> InsertAsync(Product product, List<int> categoryIds);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Guid id);

        Task<PagedData<Product>> GetProductsAsync(ProductQueryParameters queryParameters);

        Task<List<Product>> GetProductsByIdsAsync(IEnumerable<Guid> ids);

        Task<List<Category>> GetCategoriesOfProductAsync(Guid productId);   
        Task<List<Colour>> GetColoursOfProductAsync(Guid productId);   
        Task<List<ProductVariant>> GetProductVariantsAsync(Guid productId); 
        Task<ProductVariant?> GetProductVariantAsync(Guid productId, int variantNumber); 
        Task<ProductVariant?> AddVariantForProductAsync(Guid productId, ProductVariant productVariant);
        Task<List<ProductVariant>> GetProductsVariantAsync(Guid productId);
    }
}