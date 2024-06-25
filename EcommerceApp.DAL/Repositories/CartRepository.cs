using EcommerceApp.Domain.Exceptions;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.DAL.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext dbContext;

        public CartRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task AddProductsAsync(string appUserId, Guid productVariationId, int quantity)
        {
            var productVariation = await dbContext.ProductVariations.FindAsync(productVariationId);
            if (productVariation == null) throw new NotFoundException("The product variation does not exist.");

            var existingCartItem = dbContext.CartItems
                .Where(x => x.AppUserId == appUserId && x.ProductVariationId == productVariationId)
                .FirstOrDefault();

            if (existingCartItem == null)
            {
                var cartItem = new CartItem()
                {
                    ProductVariationId = productVariationId,
                    AppUserId = appUserId,
                    Quantity = quantity
                };

                dbContext.CartItems.Add(cartItem);
            }
            else
            {
                existingCartItem.Quantity += quantity;
                 
            }

            await dbContext.SaveChangesAsync();
        }

        public async Task<List<CartItem>> GetCartLinesAsync(string appUserId)
        {
            return await dbContext.CartItems.Include(x => x.ProductVariation)
                .Where(x => x.AppUserId == appUserId)
                .ToListAsync();

        }

        public async Task RemoveProductAsync(string appUserId, Guid productVariationId)
        {
            var existingCartItem = dbContext.CartItems
                .Where(x => x.AppUserId == appUserId && x.ProductVariationId == productVariationId)
                .FirstOrDefault();

            if (existingCartItem == null)
                throw new NotFoundException("The cart does not contain the product variation.");

            existingCartItem.Quantity--;

            if (existingCartItem.Quantity == 0)
            {
                dbContext.CartItems.Remove(existingCartItem);
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
