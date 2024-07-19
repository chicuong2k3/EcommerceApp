
using EcommerceApp.Common.Exceptions;
using EcommerceApp.Common.Shared;
using EcommerceApp.Common.Shared.Extensions;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace EcommerceApp.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext dbContext;

        public ProductRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await dbContext.Products
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Include(x => x.ProductItems).ThenInclude(x => x.ProductVariations)
                .Include(x => x.Categories)
                .FirstOrDefaultAsync();
        }

        public async Task<Product> InsertAsync(Product product, IEnumerable<int> categoryIds)
        {
            var categories = await dbContext.Categories
                .Where(x => categoryIds.Contains(x.Id))
                .ToListAsync();

            if (categories.Count() != categoryIds.Count())
            {
                throw new BadRequestException<IEnumerable<Category>>();
            }
            product.Categories = categories;

            foreach (var productItem in product.ProductItems)
            {
                var variations = new List<ProductVariation>();
                var variationsNeedToRemove = new List<ProductVariation>();

                foreach (var variation in productItem.ProductVariations)
                {
                    var existingVariation = await dbContext.ProductVariations
                    .Where(x => x.Name.ToLower().Equals(variation.Name.ToLower())
                    && x.Value.ToLower().Equals(variation.Value.ToLower()))
                    .FirstOrDefaultAsync();

                    if (existingVariation != null)
                    {
                        variations.Add(existingVariation);
                        variationsNeedToRemove.Add(variation);
                    }
                }

                foreach (var v in variationsNeedToRemove)
                {
                    productItem.ProductVariations.Remove(v);
                }

                foreach (var v in variations)
                {
                    productItem.ProductVariations.Add(v);
                }
            }
            
            dbContext.Products.Add(product);

            await dbContext.SaveChangesAsync();

            return product;
        }

        public async Task UpdateAsync(Product product, IEnumerable<int> categoryIds)
        {
            var exist = dbContext.Products.Any(x => x.Id == product.Id);

            if (!exist)
            {
                throw new NotFoundException<Product, Guid>(product.Id);
            }

            if (categoryIds != null && categoryIds.Any())
            {
                var categories = await dbContext.Categories
                .Where(x => categoryIds.Contains(x.Id))
                .ToListAsync();

                if (categories.Count() != categoryIds.Count())
                {
                    throw new BadRequestException<IEnumerable<Category>>();
                }

                product.Categories = categories;
            }

            foreach (var productItem in product.ProductItems)
            {
                var variations = new List<ProductVariation>();
                var variationsNeedToRemove = new List<ProductVariation>();

                foreach (var variation in productItem.ProductVariations)
                {
                    var existingVariation = await dbContext.ProductVariations
                    .Where(x => x.Name.ToLower().Equals(variation.Name.ToLower())
                    && x.Value.ToLower().Equals(variation.Value.ToLower()))
                    .FirstOrDefaultAsync();

                    if (existingVariation != null)
                    {
                        variations.Add(existingVariation);
                        variationsNeedToRemove.Add(variation);
                    }
                }

                foreach (var v in variationsNeedToRemove)
                {
                    productItem.ProductVariations.Remove(v);
                }

                foreach (var v in variations)
                {
                    productItem.ProductVariations.Add(v);
                }
            }

            dbContext.Products.Update(product);
            await dbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(Guid id)
        {
            var product = await dbContext.Products.FindAsync(id);

            if (product == null)
            {
                throw new NotFoundException<Product, Guid>(id);
            }

            dbContext.Products.Remove(product);
            await dbContext.SaveChangesAsync();
        }


        public async Task<PagedData<Product>> GetProductsAsync(ProductQueryParameters queryParameters)
        {
            string keyword = string.Empty;

            if (!string.IsNullOrEmpty(queryParameters.Keyword))
            {
                keyword = queryParameters.Keyword.Trim().ToLower();
            }

            IQueryable<Product> products;

            if (queryParameters.CategoryId == null)
            {
                var productTable = dbContext.Products
                    .Where(x => x.Name.ToLower().Contains(keyword));
                products = dbContext.ProductItems
                    .Where(x => x.BasePrice >= queryParameters.MinPrice && x.BasePrice <= queryParameters.MaxPrice)
                    .Select(x => new { x.ProductId })
                    .Join(productTable, pi => pi.ProductId, p => p.Id, (pi, p) => p)
                    .Distinct();

            }
            else
            {
                var productTable = dbContext.Products
                    .Where(x => x.Categories.Any(x => x.Id == queryParameters.CategoryId))
                    .Where(x => x.Name.ToLower().Contains(keyword));
                    
                products = dbContext.ProductItems
                    .Where(x => x.BasePrice >= queryParameters.MinPrice && x.BasePrice <= queryParameters.MaxPrice)
                    .Select(x => new { x.ProductId })
                    .Join(productTable, pi => pi.ProductId, p => p.Id, (pi, p) => p)
                    .Distinct();
            }

            var totalItems = await products.CountAsync();


            products = products.Sort(queryParameters.SortBy);

            var start = (queryParameters.Page - 1) * queryParameters.Limit;
            products = products.Skip(start).Take(queryParameters.Limit);


            products.Include(x => x.ProductItems)
                .ThenInclude(x => x.ProductVariations)
                .Include(x => x.Categories);

            return new PagedData<Product>(
                await products.AsNoTracking().ToListAsync(),
                queryParameters.Page,
                queryParameters.Limit,
                totalItems);
        }

    }
}