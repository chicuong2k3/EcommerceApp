using EcommerceApp.Domain.Models;
using EcommerceApp.Domain.Shared;

namespace EcommerceApp.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task<PagedData<Category>> GetCategoriesAsync(CategoryQueryParameters queryParameters);
        Task<Category?> GetByIdAsync(int id);
        Task<Category> InsertAsync(Category category);
        Task UpdateAsync(Category category);
        Task<bool> DeleteAsync(int id);
    }
}