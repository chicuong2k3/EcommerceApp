using EcommerceApp.BlazorWeb.Requests;
using EcommerceApp.BlazorWeb.Responses;

namespace EcommerceApp.BlazorWeb.Services
{
    internal interface IProductService
    {
        Task<PagedResult<Product>> GetProductsAsync(ProductQueryParameters queryParameters);
        Task<Product> GetProductByIdAsync(Guid id);
        Task<Product> CreateProductAsync(CreateProductRequest request);
        Task UpdateProductAsync(UpdateProductRequest request);
        Task DeleteProductAsync(Guid id);
    }
}
