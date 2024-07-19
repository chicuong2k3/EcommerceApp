
using EcommerceApp.Common.Exceptions;
using EcommerceApp.Common.Shared;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using EcommerceApp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EcommerceApp.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext dbContext;

        public CartRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<CartItem> AddProductsAsync(string cartId, Guid productItemId, int quantity)
        {
            var product = await dbContext.ProductItems
                .Where(x => x.Id == productItemId)
                .FirstOrDefaultAsync();

            if (product == null)
                throw new NotFoundException<ProductItem, Guid>();

            var cart = await dbContext.Carts.FindAsync(cartId);
            if (cart == null)
                throw new NotFoundException<Cart, string>();

            var cartItem = dbContext.CartItems
                .Where(x => x.CartId == cartId && x.ProductItemId == productItemId)
                .FirstOrDefault();

            if (cartItem == null)
            {
                cartItem = new CartItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductItemId = productItemId,
                    CartId = cartId,
                    Quantity = quantity
                };

                dbContext.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;

            }

            await dbContext.SaveChangesAsync();
            return cartItem;
        }

        public async Task ClearCartAsync(string cartId)
        {
            var cartItems = dbContext.CartItems.Where(x => x.CartId == cartId);
            dbContext.CartItems.RemoveRange(cartItems);
            await dbContext.SaveChangesAsync();
        }

        public async Task CreateCartAsync(Cart cart)
        {
            cart.Id = Guid.NewGuid().ToString();
            dbContext.Carts.Add(cart);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Cart?> GetCartByIdAsync(string cartId)
        {
            return await dbContext.Carts
                .AsNoTracking()
                .Where(x => x.Id == cartId)
                .FirstOrDefaultAsync();
        }

        public async Task<Cart?> GetCartByOwnerIdAsync(string userId)
        {

            return await dbContext.Carts.AsNoTracking()
                .Where(x => x.AppUserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task<CartItem?> GetCartItemByIdAsync(string cartItemId)
        {
            return await dbContext.CartItems
                .AsNoTracking()
                .Where(x => x.Id == cartItemId)
                .FirstOrDefaultAsync();
        }

        public async Task<PagedData<CartItem>> GetCartItemsAsync(string cartId, CartItemQueryParameters queryParameters)
        {
            var items = dbContext.CartItems
                .Where(x => x.CartId == cartId)
                .AsNoTracking();

            var totalItems = await items.CountAsync();

            var start = (queryParameters.Page - 1) * queryParameters.Limit;
            items = items.Skip(start).Take(queryParameters.Limit);


            return new PagedData<CartItem>(
                await items.AsNoTracking().ToListAsync(),
                queryParameters.Page,
                queryParameters.Limit,
                totalItems);

        }
        public async Task<string> GetCartOwnerIdAsync(string cartId)
        {
            var cart = await dbContext.Carts.FindAsync(cartId);

            if (cart == null)
                throw new NotFoundException<Cart, string>();

            return cart.AppUserId;
        }

        public async Task DecreaseProductQuantityAsync(string cartItemId, int quantity)
        {

            var existingCartItem = dbContext.CartItems
                .Where(x => x.Id == cartItemId)
                .FirstOrDefault();

            if (existingCartItem == null)
                throw new NotFoundException<CartItem, string>();

            if (existingCartItem.Quantity < quantity)
            {
                throw new QuantityExceedException("The quantity of items to remove must not exceed the quantity of items in the cart.");
            }

            existingCartItem.Quantity -= quantity;

            if (existingCartItem.Quantity == 0)
            {
                dbContext.CartItems.Remove(existingCartItem);
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
