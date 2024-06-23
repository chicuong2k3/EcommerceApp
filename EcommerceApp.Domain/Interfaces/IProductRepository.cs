using EcommerceApp.Domain.Models;
using EcommerceApp.Domain.Shared;

namespace EcommerceApp.Domain.Interfaces
{
    public interface IProductRepository
    {

        //Task<bool> RemoveFromCategoryAsync(int categoryId, int productId);
        Task<Product?> GetByIdAsync(Guid id);
        Task<Product?> CreateAsync(Product product, List<int> categoryIds, List<int> colorIds, Dictionary<int, List<ProductVariation>> optionsForColour);
        Task<bool> UpdateAsync(Product product);
        Task<bool> DeleteAsync(Guid id);

        Task<PagingData<Product>> GetProductsAsync(ProductQueryParameters queryParameters);

        Task<List<Product>> GetProductsByIdsAsync(IEnumerable<Guid> ids);

        Task<List<Category>> GetCategoriesOfProduct(Guid productId);   
        Task<List<Colour>> GetColoursOfProduct(Guid productId);   
        Task<List<ProductVariation>> GetOptionsForColor(Guid productId, int colorId);   
    }
}