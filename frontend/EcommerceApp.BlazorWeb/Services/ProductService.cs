using EcommerceApp.BlazorWeb.Extensions;
using EcommerceApp.BlazorWeb.Requests;
using EcommerceApp.BlazorWeb.Responses;
using System.Net.Http.Json;

namespace EcommerceApp.BlazorWeb.Services
{
    internal class ProductService : IProductService
    {
        private readonly HttpClient httpClient;
        private const string endpoint = "api/products";

        public ProductService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<PagedResult<Product>> GetProductsAsync(ProductQueryParameters queryParameters)
        {
            var url = $"{endpoint}?" +
                (string.IsNullOrEmpty(queryParameters.Keyword) ? string.Empty : $"keyword={queryParameters.Keyword}&") +
                (queryParameters.Page <= 0 ? string.Empty : $"page={queryParameters.Page}&") +
                (queryParameters.Limit <= 0 ? string.Empty : $"limit={queryParameters.Limit}&") +
                (string.IsNullOrEmpty(queryParameters.SortBy) ? string.Empty : $"sortBy={queryParameters.SortBy}");
            var response = await httpClient.GetAsync(url);
            var temp = await response.ToClassInstance<PagedResult<Product>>();
            return temp;
        }

        public async Task<Product> CreateProductAsync(CreateProductRequest request)
        {
            var response = await httpClient.PostAsJsonAsync($"{endpoint}", request);
            return await response.ToClassInstance<Product>();
        }

        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            var response = await httpClient.GetAsync($"{endpoint}/{id}");
            return await response.ToClassInstance<Product>();
        }

        public async Task DeleteProductAsync(Guid id)
        {
            await httpClient.DeleteAsync($"{endpoint}/{id}");
        }

        public async Task UpdateProductAsync(UpdateProductRequest request)
        {
            await httpClient.PutAsJsonAsync($"{endpoint}/{request.Id}", request);
        }
    }
}
