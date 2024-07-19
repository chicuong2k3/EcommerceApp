using EcommerceApp.Common.Shared;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(Guid id); 
        Task<Order?> CreateAsync(Order order, List<OrderLine> orderLines);
        Task<PagedData<Order>> GetOrdersByCustomerIdAsync(string userId, OrderQueryParameters queryParameters);
        Task<PagedData<OrderLine>> GetOrderLinesAsync(Guid orderId, OrderLineQueryParameters queryParameters);

        Task UpdateOrderStatus(Guid orderId, int orderStatusId);
        Task<int> GetOrderStatusIdByValue(string orderStatus);
    }
}
