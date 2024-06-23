using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
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

        public async Task<Category> InsertAsync(Category category)
        {
            dbContext.Categories.Add(category);
            await dbContext.SaveChangesAsync();
            return category;
        }

        public async Task UpdateAsync(Category category)
        {
            dbContext.Categories.Update(category);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await dbContext.Categories.FindAsync(id);

            if (category == null)
            {
                return false;
            }

            dbContext.Categories.Remove(category);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            var category = await dbContext.Categories.AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return category;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await dbContext.Categories.AsNoTracking().ToListAsync();
        }

        
    }
}