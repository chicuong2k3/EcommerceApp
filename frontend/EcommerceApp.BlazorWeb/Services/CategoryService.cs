
using EcommerceApp.BlazorWeb.Extensions;
using EcommerceApp.BlazorWeb.Requests;
using EcommerceApp.BlazorWeb.Responses;
using System;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace EcommerceApp.BlazorWeb.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient httpClient;
        private const string endpoint = "api/categories";

        public CategoryService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await httpClient.DeleteAsync($"{endpoint}/{id}");
        }

        public async Task<PagedResult<Category>> GetCategoriesAsync(CategoryQueryParameters queryParameters)
        {
            var keyword = string.IsNullOrEmpty(queryParameters.Keyword) ? string.Empty : queryParameters.Keyword;
            var url = $"{endpoint}?" +
                $"keyword={keyword}" + 
                (queryParameters.Page <= 0 ? string.Empty : $"&page={queryParameters.Page}") +
                (queryParameters.Limit <= 0 ? string.Empty : $"&limit={queryParameters.Limit}") +
                (string.IsNullOrEmpty(queryParameters.SortBy) ? string.Empty : $"&sortBy={queryParameters.SortBy}");
            var response = await httpClient.GetAsync(url);
            var temp = await response.ToClassInstance<PagedResult<Category>>();
            return temp;

        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            var response = await httpClient.GetAsync($"{endpoint}/{id}");
            return await response.ToClassInstance<Category>();
        }

        public async Task<Category> CreateCategoryAsync(CreateCategoryRequest request)
        {
            var response = await httpClient.PostAsJsonAsync($"{endpoint}", request);
            return await response.ToClassInstance<Category>();
        }

        public async Task UpdateCategoryAsync(UpdateCategoryRequest request)
        {
            await httpClient.PutAsJsonAsync($"{endpoint}/{request.Id}", request);
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            var response = await httpClient.GetAsync($"{endpoint}/all");
            var temp = await response.ToClassInstance<List<Category>>();
            return temp;
        }
    }
}
