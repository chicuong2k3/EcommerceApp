using EcommerceApp.Common.Shared;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Domain.Interfaces
{
    public interface IReviewRepository
    {
        Task<PagedData<Review>> GetReviewsAsync(ReviewQueryParameters queryParameters);
        Task<Review?> GetByIdAsync(Guid id);
        Task<Review> InsertAsync(Review review);
        Task UpdateAsync(Review review);
        Task DeleteAsync(Guid id);
    }
}