using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using EcommerceApp.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EcommerceApp.DAL.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<PagingData<Product>> GetProductsAsync(ProductQueryParameters queryParameters)
        {
            if (queryParameters.CategoryId == null)
            {
                var products = await dbContext.Products
                            .Where(x => x.Price >= queryParameters.MinPrice && x.Price <= queryParameters.MaxPrice)
                            .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                            .Take(queryParameters.PageSize)
                            .ToListAsync();

                return new PagingData<Product>(products, queryParameters.PageNumber, queryParameters.PageSize);
            }
            else
            {
                var products = await dbContext.Products
                            .Where(x => x.CategoryId == queryParameters.CategoryId
                            && x.Price >= queryParameters.MinPrice && x.Price <= queryParameters.MaxPrice)
                            .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                            .Take(queryParameters.PageSize)
                            .ToListAsync();

                return new PagingData<Product>(products, queryParameters.PageNumber, queryParameters.PageSize);
            }

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