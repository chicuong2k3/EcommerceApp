namespace EcommerceApp.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);

        Task<T> InsertAsync(T entity);

        Task<bool> UpdateAsync(int id, T updatedEntity);

        Task<bool> DeleteAsync(int id);
    }
}