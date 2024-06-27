using EcommerceApp.Domain.Exceptions;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using EcommerceApp.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EcommerceApp.DAL.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext dbContext;

        public CartRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<CartItem?> AddProductsAsync(Guid cartId, Guid productId, int variantNumber, int quantity)
        {
            var productVariant = await dbContext.ProductVariants
                .Where(x => x.ProductId == productId 
                && x.VariantNumber == variantNumber)
                .FirstOrDefaultAsync();

            if (productVariant == null) 
                throw new NotFoundException("The product variation does not exist.");
    
            var cart = await dbContext.Carts.FindAsync(cartId);
            if (cart == null)
                throw new NotFoundException("The cart does not exist.");

            var cartItem = dbContext.CartItems
                .Where(x => x.CartId == cartId && x.ProductId == productId && x.VariantNumber == variantNumber)
                .FirstOrDefault();

            if (cartItem == null)
            {
                cartItem = new CartItem()
                {
                    ProductId = productId,
                    VariantNumber = variantNumber,
                    CartId = cartId,
                    Quantity = quantity
                };

                dbContext.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
                 
            }

            try
            {
                await dbContext.SaveChangesAsync();
                return cartItem;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task ClearCartAsync(Guid cartId)
        {
            var cartItems = dbContext.CartItems.Where(x => x.CartId == cartId);
            dbContext.CartItems.RemoveRange(cartItems);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Cart?> CreateCartAsync(Cart cart)
        {
            try
            {
                dbContext.Carts.Add(cart);  
                await dbContext.SaveChangesAsync();
                return cart;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Cart?> GetCartByIdAsync(Guid cartId)
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

        public async Task<CartItem?> GetCartItemByIdAsync(Guid cartItemId)
        {
            return await dbContext.CartItems
                .AsNoTracking()
                .Where(x => x.Id == cartItemId)
                .FirstOrDefaultAsync();
        }

        public async Task<PagedData<CartItem>> GetCartItemsAsync(Guid cartId, CartItemQueryParameters queryParameters)
        {
            var items = dbContext.CartItems.AsNoTracking();
            

            var totalItems = await items.CountAsync();

            var start = (queryParameters.Page - 1) * queryParameters.Limit;
            items = items.Skip(start).Take(queryParameters.Limit);


            return new PagedData<CartItem>(
                await items.AsNoTracking().ToListAsync(),
                queryParameters.Page,
                queryParameters.Limit,
                totalItems);

        }
        public async Task<string> GetCartOwnerIdAsync(Guid cartId)
        {
            var cart = await dbContext.Carts.FindAsync(cartId);

            if (cart == null)
                throw new NotFoundException("The cart does not exist.");

            return cart.AppUserId;
        }

        public async Task RemoveProductAsync(Guid cartItemId, int quantity)
        {
            
            var existingCartItem = dbContext.CartItems
                .Where(x => x.Id == cartItemId)
                .FirstOrDefault();

            if (existingCartItem == null)
                throw new NotFoundException("The cart item does not exist.");

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
