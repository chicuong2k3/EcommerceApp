using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using EcommerceApp.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.DAL.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<PagingData<Product>> GetAllProductsAsync(int pageNumber, int pageSize)
        {
            var products = (await GetAllAsync())
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();
            return new PagingData<Product>(products, pageNumber, pageSize);
        }

        public async Task<PagingData<Product>> GetProductsByCategoryAsync(int categoryId, int pageNumber, int pageSize)
        {
            var products = await dbContext.Products
                .Where(x => x.CategoryId == categoryId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new PagingData<Product>(products, pageNumber, pageSize);
        }

        public async Task<List<Product>> GetProductsByIdsAsync(IEnumerable<int> ids)
        {
            var products = await dbContext.Products
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
            return products;
        }
    }
}