using EcommerceApp.Domain.Models;
using EcommerceApp.Domain.Shared;

namespace EcommerceApp.Domain.Interfaces
{
    public interface IProductRepository
    {

        //Task<bool> RemoveFromCategoryAsync(int categoryId, int productId);
        Task<Product?> GetByIdAsync(Guid id);
        Task<Product?> CreateAsync(Product product, List<int> categoryIds, List<int> colorIds, Dictionary<int, List<ProductVariant>> optionsForColour);
        Task<bool> UpdateAsync(Product product);
        Task<bool> DeleteAsync(Guid id);

        Task<PagedData<Product>> GetProductsAsync(ProductQueryParameters queryParameters);

        Task<List<Product>> GetProductsByIdsAsync(IEnumerable<Guid> ids);

        Task<List<Category>> GetCategoriesOfProductAsync(Guid productId);   
        Task<List<Colour>> GetColoursOfProductAsync(Guid productId);   
        Task<List<ProductVariant>> GetOptionsForColorAsync(Guid productId, int colorId); 
        Task<ProductVariant?> GetProductVariantByIdAsync(Guid productVariantId); 
        
        Task<ProductItem?> GetProductItemByIdAsync(Guid productItemId);
    }
}