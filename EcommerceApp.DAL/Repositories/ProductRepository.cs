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

        public async Task<Product?> CreateAsync(Product product, List<int> categoryIds, List<int> colorIds, Dictionary<int, List<ProductVariant>> optionsForColour)
        {
            product.ProductItems = colorIds.Select(colorId =>
            {
                return new ProductItem() { ColourId = colorId };
            }).ToList();

            product.ProductCategories = categoryIds.Select(categoryId =>
            {
                return new ProductCategory() { CategoryId = categoryId };
            }).ToList();

            foreach (var colourId in optionsForColour.Keys)
            {
                var pi = product.ProductItems.Where(x => x.ColourId == colourId).FirstOrDefault();
                if (pi != null)
                {
                    pi.ProductVariants = optionsForColour[colourId];
                }
            }

            dbContext.Products.Add(product);

            var writtenEntries = await dbContext.SaveChangesAsync();

            if (writtenEntries <= 0) return null;

            return product;
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            var product = await dbContext.Products.FindAsync(id);

            if (product == null)
            {
                return false;
            }

            dbContext.Products.Remove(product);
            var writtenEntries = await dbContext.SaveChangesAsync();
            return writtenEntries > 0;
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            dbContext.Products.Update(product);
            var writtenEntries = await dbContext.SaveChangesAsync();
            return writtenEntries > 0;
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

            IQueryable<ProductItem> productItemTable = dbContext.ProductItems;

            if (queryParameters.Colours != null && queryParameters.Colours.Count() > 0)
            {
                productItemTable = productItemTable.Where(x => queryParameters.Colours.Contains(x.ColourId));
                    
            }

            if (queryParameters.Sizes != null && queryParameters.Sizes.Count() > 0)
            {
                productItemTable = dbContext.ProductVariants
                    .Where(x => queryParameters.Sizes.Contains(x.SizeId))
                    .Select(x => new { x.ProductItemId })
                    .Join(productItemTable, pv => pv.ProductItemId, pi => pi.Id, (pv, pi) => pi)
                    .Distinct();
            }

            if (queryParameters.CategoryId == null)
            {
                var productTable = dbContext.Products
                    .Where(x => x.Name.ToLower().Contains(keyword)
                && x.OriginalPrice >= queryParameters.MinPrice
                && x.OriginalPrice <= queryParameters.MaxPrice);
     
                products = productItemTable.Select(x => new { x.ProductId })
                    .Join(productTable, pi => pi.ProductId, p => p.Id, (pi, p) => p)
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

                products = productItemTable.Select(x => new { x.ProductId })
                    .Join(productTable, pi => pi.ProductId, p => p.Id, (pi, p) => p)
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
                .Join(dbContext.Categories.AsNoTracking(), pc => pc.CategoryId, c => c.Id, (pc, c) => c)
                .Distinct()
                .ToListAsync();
                    
        }

        public async Task<List<Colour>> GetColoursOfProductAsync(Guid productId)
        {
            return await dbContext.ProductItems
                .AsNoTracking()
                .Where(x => x.ProductId == productId)
                .Join(dbContext.Colours.AsNoTracking(), pi => pi.ColourId, c => c.Id, (pi, c) => c)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<ProductVariant>> GetOptionsForColorAsync(Guid productId, int colorId)
        {
            return await dbContext.ProductVariants
                .AsNoTracking()
                .Join(
                dbContext.ProductItems.AsNoTracking().Where(x => x.ProductId == productId),
                pv => pv.ProductItemId,
                pi => pi.Id,
                (pv, pi) => pv)
                .Include(x => x.Size)
                .Distinct()
                .ToListAsync();
        }

        public async Task<ProductItem?> GetProductItemByIdAsync(Guid productItemId)
        {
            return await dbContext.ProductItems
                .AsNoTracking()
                .Include(x => x.Colour)
                .Where(x => x.Id == productItemId)
                .FirstOrDefaultAsync();
        }

        public async Task<ProductVariant?> GetProductVariantByIdAsync(Guid productVariantId)
        {
            return await dbContext.ProductVariants
                .AsNoTracking()
                .Include(x => x.Size)
                .Where(x => x.Id == productVariantId)
                .FirstOrDefaultAsync();
        }
    }
}