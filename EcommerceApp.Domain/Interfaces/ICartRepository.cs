using EcommerceApp.Domain.Models;

namespace EcommerceApp.Domain.Interfaces
{
    public interface ICartRepository
    {
        Task<List<CartItem>> GetCartLinesAsync(string appUserId);
        Task AddProductsAsync(string appUserId, Guid productVariationId, int quantity);
        Task RemoveProductAsync(string appUserId, Guid productVariationId);
    }
}
