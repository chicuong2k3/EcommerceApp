using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.DAL.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId, int pageSize, int pageNumber)
        {
            var products = await dbContext.Products
                .Where(x => x.CategoryId == categoryId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return products;
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