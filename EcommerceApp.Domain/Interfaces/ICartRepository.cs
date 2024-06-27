using EcommerceApp.Domain.Models;
using EcommerceApp.Domain.Shared;

namespace EcommerceApp.Domain.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartByIdAsync(Guid cartId);
        Task<Cart?> CreateCartAsync(Cart cart);
        Task<CartItem?> GetCartItemByIdAsync(Guid cartItemId);
        Task<PagedData<CartItem>> GetCartItemsAsync(Guid cartId, CartItemQueryParameters queryParameters);
        Task<CartItem?> AddProductsAsync(Guid cartId, Guid productId, int variantNumber, int quantity);
        Task RemoveProductAsync(Guid cartItemId, int quantity);
        Task<string> GetCartOwnerIdAsync(Guid cartId);
        Task<Cart?> GetCartByOwnerIdAsync(string userId);
        Task ClearCartAsync(Guid cartId);
    }
}
