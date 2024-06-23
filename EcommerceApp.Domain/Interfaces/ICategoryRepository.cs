using EcommerceApp.Domain.Models;

namespace EcommerceApp.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetCategoriesAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<Category> InsertAsync(Category category);
        Task UpdateAsync(Category category);
        Task<bool> DeleteAsync(int id);
    }
}