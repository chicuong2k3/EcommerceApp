using EcommerceApp.Domain.Exceptions;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using EcommerceApp.Domain.Shared;
using EcommerceApp.Domain.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.DAL.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext dbContext;

        public ReviewRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Review?> InsertAsync(Review review)
        {
            try
            {
                dbContext.Reviews.Add(review);
                await dbContext.SaveChangesAsync();
                return review;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task UpdateAsync(Review review)
        {
            var exist = dbContext.Reviews.Any(x => x.Id == review.Id);

            if (!exist)
            {
                throw new NotFoundException("Cannot find the review to update.");
            }

            dbContext.Reviews.Update(review);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var review = await dbContext.Reviews.FindAsync(id);

            if (review == null)
            {
                throw new NotFoundException("Cannot find the review to delete.");
            }

            dbContext.Reviews.Remove(review);
            await dbContext.SaveChangesAsync();

        }

        public async Task<Review?> GetByIdAsync(Guid id)
        {
            var review = await dbContext.Reviews.AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return review;
        }

        public async Task<PagedData<Review>> GetReviewsAsync(ReviewQueryParameters queryParameters)
        {
            var reviews = dbContext.Reviews.AsNoTracking();
            //if (!string.IsNullOrEmpty(queryParameters.Keyword))
            //{
            //    var keyword = queryParameters.Keyword.ToLower();
            //    reviews = reviews.Where(x => x.Name.ToLower().Contains(keyword));
            //}

            var totalItems = await reviews.CountAsync();

            var start = (queryParameters.Page - 1) * queryParameters.Limit;
            reviews = reviews.Skip(start).Take(queryParameters.Limit);

            reviews = reviews.Sort(queryParameters.SortBy);

            return new PagedData<Review>(
                await reviews.AsNoTracking().ToListAsync(),
                queryParameters.Page,
                queryParameters.Limit,
                totalItems);
        }

        
    }
}