using EcommerceApp.Common.Shared;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task<PagedData<Category>> GetCategoriesAsync(CategoryQueryParameters queryParameters);
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<Category> InsertAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(int id);
    }
}