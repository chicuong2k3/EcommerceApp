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
        public async Task<PagingData<Product>> GetProductsAsync(ProductQueryParameters queryParameters)
        {
            string keyword =  string.Empty;

            if (!string.IsNullOrEmpty(queryParameters.Keyword))
            {
                keyword = queryParameters.Keyword.Trim().ToLower();
            }

            if (queryParameters.CategoryId == null)
            {
                var products = dbContext.Products
                            .Where(x =>
                            x.Price >= queryParameters.MinPrice && x.Price <= queryParameters.MaxPrice
                            && x.Name.ToLower().Contains(keyword))
                            .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                            .Take(queryParameters.PageSize);

                products = products.Sort(queryParameters.OrderBy);
                var res = await products.ToListAsync();

                return new PagingData<Product>(res, queryParameters.PageNumber, queryParameters.PageSize);
            }
            else
            {
                var products = dbContext.Products
                            .Where(x =>
                            x.CategoryId == queryParameters.CategoryId
                            && x.Price >= queryParameters.MinPrice && x.Price <= queryParameters.MaxPrice
                            && x.Name.ToLower().Contains(keyword))
                            .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                            .Take(queryParameters.PageSize);

                products = products.Sort(queryParameters.OrderBy);
                var res = await products.ToListAsync();

                return new PagingData<Product>(res, queryParameters.PageNumber, queryParameters.PageSize);
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