using EcommerceApp.Domain.Models;

namespace EcommerceApp.Domain.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        //Task<bool> AddToCategoryAsync(int categoryId, int productId);

        //Task<bool> RemoveFromCategoryAsync(int categoryId, int productId);

        Task<List<Product>> GetProductsByCategoryAsync(int categoryId);
    }
}