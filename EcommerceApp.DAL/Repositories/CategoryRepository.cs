using EcommerceApp.Domain.Exceptions;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using EcommerceApp.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.DAL.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext dbContext;

        public CategoryRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Category?> InsertAsync(Category category)
        {
            try
            {
                dbContext.Categories.Add(category);
                await dbContext.SaveChangesAsync();
                return category;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task UpdateAsync(Category category)
        {
            var exist = dbContext.Categories.Any(x => x.Id == category.Id);

            if (!exist)
            {
                throw new NotFoundException("Cannot find the category to update.");
            }

            category.Slug = category.Name.GenerateSlug();
            dbContext.Categories.Update(category);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await dbContext.Categories.FindAsync(id);

            if (category == null)
            {
                throw new NotFoundException("Cannot find the category to delete.");
            }

            dbContext.Categories.Remove(category);
            await dbContext.SaveChangesAsync();

        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            var category = await dbContext.Categories.AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return category;
        }

        public async Task<PagedData<Category>> GetCategoriesAsync(CategoryQueryParameters queryParameters)
        {
            var categories = dbContext.Categories.AsNoTracking();
            if (!string.IsNullOrEmpty(queryParameters.Keyword))
            {
                var keyword = queryParameters.Keyword.ToLower();
                categories = categories.Where(x => x.Name.ToLower().Contains(keyword));
            }

            var totalItems = await categories.CountAsync();

            var start = (queryParameters.Page - 1) * queryParameters.Limit;
            categories = categories.Skip(start).Take(queryParameters.Limit);

            categories = categories.Sort(queryParameters.SortBy);

            return new PagedData<Category>(
                await categories.AsNoTracking().ToListAsync(),
                queryParameters.Page,
                queryParameters.Limit,
                totalItems);
        }

        
    }
}