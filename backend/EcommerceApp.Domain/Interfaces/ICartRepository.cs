using EcommerceApp.Common.Shared;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Domain.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartByIdAsync(string cartId);
        Task CreateCartAsync(Cart cart);
        Task<CartItem?> GetCartItemByIdAsync(string cartItemId);
        Task<PagedData<CartItem>> GetCartItemsAsync(string cartId, CartItemQueryParameters queryParameters);
        Task<CartItem> AddProductsAsync(string cartId, Guid productItemId, int quantity);
        Task DecreaseProductQuantityAsync(string cartItemId, int quantity);
        Task<string> GetCartOwnerIdAsync(string cartId);
        Task<Cart?> GetCartByOwnerIdAsync(string userId);
        Task ClearCartAsync(string cartId);
    }
}
