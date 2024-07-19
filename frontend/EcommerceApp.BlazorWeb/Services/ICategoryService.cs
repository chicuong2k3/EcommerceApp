using EcommerceApp.BlazorWeb.Requests;
using EcommerceApp.BlazorWeb.Responses;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace EcommerceApp.BlazorWeb.Services
{
    public interface ICategoryService
    {
        Task<PagedResult<Category>> GetCategoriesAsync(CategoryQueryParameters queryParameters);
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int id);
        Task<Category> CreateCategoryAsync(CreateCategoryRequest request);
        Task UpdateCategoryAsync(UpdateCategoryRequest request);
        Task DeleteCategoryAsync(int id);
    }
}
