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

        public async Task<Product?> CreateAsync(Product product, List<int> categoryIds, List<int> colorIds, Dictionary<int, List<ProductVariation>> optionsForColour)
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
                    pi.ProductVariations = optionsForColour[colourId];
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
            return await dbContext.Products.FindAsync(id);
        }

        public async Task<PagingData<Product>> GetProductsAsync(ProductQueryParameters queryParameters)
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
                productItemTable = dbContext.ProductVariations.Where(x => queryParameters.Sizes.Contains(x.SizeId))
                    .Join(productItemTable, pv => pv.ProductItemId, pi => pi.Id, (pv, pi) => pi)
                    .Distinct();
            }

            if (queryParameters.CategoryId == null)
            {
                var productTable = dbContext.Products.Where(x => x.Name.ToLower().Contains(keyword)
                && x.OriginalPrice >= queryParameters.MinPrice
                && x.OriginalPrice <= queryParameters.MaxPrice);
     
                products = productItemTable.Join(productTable, pi => pi.ProductId, p => p.Id, (pi, p) => p)
                    .Distinct();

            }
            else
            {
                var productTable = dbContext.ProductCategories.Where(x => x.CategoryId == queryParameters.CategoryId)
                    .Join(dbContext.Products.Where(x => x.Name.ToLower().Contains(keyword)
                && x.OriginalPrice >= queryParameters.MinPrice
                && x.OriginalPrice <= queryParameters.MaxPrice), pc => pc.ProductId, p => p.Id, (pc, p) => p)
                    .Distinct();
        
                products = productItemTable.Join(productTable, pi => pi.ProductId, p => p.Id, (pi, p) => p)
                    .Distinct();
            }

            var totalItems = await products.CountAsync();

            products = products.Sort(queryParameters.OrderBy);

            products = products.Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                            .Take(queryParameters.PageSize);

            return new PagingData<Product>(
                await products.ToListAsync(), 
                queryParameters.PageNumber, 
                queryParameters.PageSize, 
                totalItems);
        }

        public async Task<List<Product>> GetProductsByIdsAsync(IEnumerable<Guid> ids)
        {
            var products = await dbContext.Products
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
            return products;
        }

        public async Task<List<Category>> GetCategoriesOfProduct(Guid productId)
        {
            return await dbContext.ProductCategories.Where(x => x.ProductId == productId)
                .Join(dbContext.Categories, pc => pc.CategoryId, c => c.Id, (pc, c) => c)
                .ToListAsync();
                    
        }

        public async Task<List<Colour>> GetColoursOfProduct(Guid productId)
        {
            return await dbContext.ProductItems.Where(x => x.ProductId == productId)
                .Join(dbContext.Colours, pi => pi.ColourId, c => c.Id, (pi, c) => c)
                .ToListAsync();
        }

        public async Task<List<ProductVariation>> GetOptionsForColor(Guid productId, int colorId)
        {
            return await dbContext.ProductVariations.Join(
                dbContext.ProductItems.Where(x => x.ProductId == productId),
                pv => pv.ProductItemId,
                pi => pi.Id,
                (pv, pi) => pv).ToListAsync();
        }
    }
}