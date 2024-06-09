namespace EcommerceApp.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();

        Task<T?> GetByIdAsync(int id);

        Task<T> InsertAsync(T entity);

        Task UpdateAsync(int id, T updatedEntity);

        Task DeleteAsync(int id);
    }
}