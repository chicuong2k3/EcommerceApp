using EcommerceApp.Domain.Exceptions;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using EcommerceApp.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.DAL.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext dbContext;

        public ProductRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Product?> InsertAsync(Product product, List<int> categoryIds)
        {
            try
            {
                product.ProductCategories = categoryIds.Select(categoryId =>
                {
                    return new ProductCategory() { CategoryId = categoryId };
                }).ToList();

                dbContext.Products.Add(product);

                await dbContext.SaveChangesAsync();

                return product;
            }
            catch (Exception)
            {
                return null;
            }

        }
        public async Task DeleteAsync(Guid id)
        {
            var product = await dbContext.Products.FindAsync(id);

            if (product == null)
            {
                throw new NotFoundException("Cannot find the product to delete.");
            }

            dbContext.Products.Remove(product);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            var exist = dbContext.Products.Any(x => x.Id == product.Id);

            if (!exist)
            {
                throw new NotFoundException("Cannot find the product to update.");
            }

            dbContext.Products.Update(product);
            await dbContext.SaveChangesAsync();
        }
        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await dbContext.Products
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<PagedData<Product>> GetProductsAsync(ProductQueryParameters queryParameters)
        {
            string keyword = string.Empty;

            if (!string.IsNullOrEmpty(queryParameters.Keyword))
            {
                keyword = queryParameters.Keyword.Trim().ToLower();
            }

            IQueryable<Product> products;

            IQueryable<ProductVariant> productVariantTable = dbContext.ProductVariants;

            if (queryParameters.Colours != null && queryParameters.Colours.Count() > 0)
            {
                productVariantTable = productVariantTable.Where(x => queryParameters.Colours.Contains(x.ColourId));
            }

            if (queryParameters.Sizes != null && queryParameters.Sizes.Count() > 0)
            {
                productVariantTable = productVariantTable.Where(x => queryParameters.Sizes.Contains(x.SizeId));
            }

            if (queryParameters.CategoryId == null)
            {
                var productTable = dbContext.Products
                    .Where(x => x.Name.ToLower().Contains(keyword)
                && x.OriginalPrice >= queryParameters.MinPrice
                && x.OriginalPrice <= queryParameters.MaxPrice);
     
                products = productVariantTable.Select(x => new { x.ProductId })
                    .Join(productTable, pv => pv.ProductId, p => p.Id, (pv, p) => p)
                    .Distinct();

            }
            else
            {
                var productTable = dbContext.ProductCategories
                    .Where(x => x.CategoryId == queryParameters.CategoryId)
                    .Select(x => new { x.ProductId })
                    .Join(dbContext.Products.Where(x => x.Name.ToLower().Contains(keyword)
                && x.OriginalPrice >= queryParameters.MinPrice
                && x.OriginalPrice <= queryParameters.MaxPrice)
                    , pc => pc.ProductId, p => p.Id, (pc, p) => p)
                    .Distinct();

                products = productVariantTable.Select(x => new { x.ProductId })
                    .Join(productTable, pv => pv.ProductId, p => p.Id, (pv, p) => p)
                    .Distinct();
            }

            var totalItems = await products.CountAsync();

            

            var start = (queryParameters.Page - 1) * queryParameters.Limit;
            //var end = queryParameters.PageNumber * queryParameters.PageSize;
            products = products.Skip(start).Take(queryParameters.Limit);

            products = products.Sort(queryParameters.SortBy);

            return new PagedData<Product>(
                await products.AsNoTracking().ToListAsync(), 
                queryParameters.Page, 
                queryParameters.Limit, 
                totalItems);
        }

        public async Task<List<Product>> GetProductsByIdsAsync(IEnumerable<Guid> ids)
        {
            var products = await dbContext.Products
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
            return products;
        }

        public async Task<List<Category>> GetCategoriesOfProductAsync(Guid productId)
        {
            return await dbContext.ProductCategories
                .AsNoTracking()
                .Where(x => x.ProductId == productId)
                .Select(x => new { x.CategoryId })
                .Join(dbContext.Categories.AsNoTracking(), pc => pc.CategoryId, c => c.Id, (pc, c) => c)
                .Distinct()
                .ToListAsync();
                    
        }

        public async Task<List<Colour>> GetColoursOfProductAsync(Guid productId)
        {
            return await dbContext.ProductVariants
                .AsNoTracking()
                .Where(x => x.ProductId == productId)
                .Select(x => new { x.ColourId })
                .Join(dbContext.Colours.AsNoTracking(), pv => pv.ColourId, c => c.Id, (pv, c) => c)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<ProductVariant>> GetProductVariantsAsync(Guid productId)
        {
            return await dbContext.ProductVariants
                .AsNoTracking()
                .Where (x => x.ProductId == productId)
                .Include(x => x.Size)
                .ToListAsync();
        }

        public async Task<ProductVariant?> GetProductVariantAsync(Guid productId, int variantNumber)
        {
            return await dbContext.ProductVariants
                .AsNoTracking()
                .Include(x => x.Colour)
                .Where(x => x.ProductId == productId && x.VariantNumber == variantNumber)
                .FirstOrDefaultAsync();
        }

        public async Task<ProductVariant?> AddVariantForProductAsync(Guid productId, ProductVariant productVariant)
        {
            var product = await dbContext.Products.FindAsync(productId);

            if (product == null)
            {
                throw new NotFoundException("Cannot add a product variant as the product is not found.");
            }

            try
            {
                dbContext.ProductVariants.Add(productVariant);
                await dbContext.SaveChangesAsync();
                return productVariant;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<ProductVariant>> GetProductsVariantAsync(Guid productId)
        {
            var product = await dbContext.Products.FindAsync(productId);

            if (product == null)
            {
                throw new NotFoundException("Cannot add a product variant as the product is not found.");
            }

            return await dbContext.ProductVariants
                .AsNoTracking()
                .Where(x => x.ProductId == productId)
                .ToListAsync();
        }
    }
}