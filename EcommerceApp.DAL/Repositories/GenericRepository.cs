using EcommerceApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.DAL.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext dbContext;

        protected GenericRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<T> InsertAsync(T entity)
        {
            dbContext.Set<T>().Add(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await dbContext.Set<T>().FindAsync(id);

            if (entity == null)
            {
                return false;
            }

            dbContext.Set<T>().Remove(entity);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            var entity = await dbContext.Set<T>().FindAsync(id);
            return entity;
        }

        public async Task<bool> UpdateAsync(int id, T updatedEntity)
        {
            var existingEntity = await dbContext.Set<T>().FindAsync(id);
            if (existingEntity == null)
            {
                return false;
            }

            dbContext.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);

            await dbContext.SaveChangesAsync();

            return true;
        }
    }
}