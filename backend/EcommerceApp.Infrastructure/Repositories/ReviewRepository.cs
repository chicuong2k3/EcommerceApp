using EcommerceApp.Common.Exceptions;
using EcommerceApp.Common.Shared;
using EcommerceApp.Common.Shared.Extensions;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Infrastructure.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext dbContext;

        public ReviewRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Review> InsertAsync(Review review)
        {
            dbContext.Reviews.Add(review);
            await dbContext.SaveChangesAsync();
            return review;
        }

        public async Task UpdateAsync(Review review)
        {
            var exist = dbContext.Reviews.Any(x => x.Id == review.Id);

            if (!exist)
            {
                throw new NotFoundException<Review, Guid>(review.Id);
            }

            dbContext.Reviews.Update(review);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var review = await dbContext.Reviews.FindAsync(id);

            if (review == null)
            {
                throw new NotFoundException<Review, Guid>(id);
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

            reviews = reviews.Sort(queryParameters.SortBy);

            var start = (queryParameters.Page - 1) * queryParameters.Limit;
            reviews = reviews.Skip(start).Take(queryParameters.Limit);

            

            return new PagedData<Review>(
                await reviews.AsNoTracking().ToListAsync(),
                queryParameters.Page,
                queryParameters.Limit,
                totalItems);
        }


    }
}