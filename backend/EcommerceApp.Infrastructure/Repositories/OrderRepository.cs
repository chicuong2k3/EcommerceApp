//using EcommerceApp.Domain.Constants;
//using EcommerceApp.Domain.Exceptions;
//using EcommerceApp.Domain.Interfaces;
//using EcommerceApp.Domain.Models;
//using EcommerceApp.Domain.Shared;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.VisualBasic;

//namespace EcommerceApp.Infrastructure.Repositories
//{
//    public class OrderRepository : IOrderRepository
//    {
//        private readonly AppDbContext dbContext;

//        public OrderRepository(AppDbContext dbContext)
//        {
//            this.dbContext = dbContext;
//        }

//        public async Task<Order?> CreateAsync(Order order, List<OrderLine> orderLines)
//        {
//            order.OrderStatusId = 1;
//            order.OrderDate = DateTime.Now;

//            for (int i = 0; i < orderLines.Count; i++)
//            {
//                var product = await dbContext.Products
//                    .Where(x => x.Id == orderLines[i].ProductId)
//                    .FirstAsync();

//                orderLines[i].Price = product.SalePrice * orderLines[i].Quantity;
//            }

//            order.Total = orderLines.Sum(x => x.Price);
//            order.OrderLines = orderLines;

//            dbContext.Orders.Add(order);

//            try
//            {
//                await dbContext.SaveChangesAsync();
//                return order;
//            }
//            catch (Exception)
//            {
//                return null;
//            }
//        }

//        public async Task<Order?> GetByIdAsync(Guid id)
//        {
//            return await dbContext.Orders
//                .AsNoTracking()
//                .Where(x => x.Id == id)
//                .FirstOrDefaultAsync();
//        }

//        public async Task<PagedData<OrderLine>> GetOrderLinesAsync(Guid orderId, OrderLineQueryParameters queryParameters)
//        {
//            var items = dbContext.OrderLines.Where(x => x.OrderId == orderId);

//            var total = await items.CountAsync();

//            var start = (queryParameters.Page - 1) * queryParameters.Limit;
//            items = items.Skip(start).Take(queryParameters.Limit);

//            return new PagedData<OrderLine>(
//                await items.AsNoTracking().ToListAsync(),
//                queryParameters.Page,
//                queryParameters.Limit,
//                total);
//        }

//        public async Task<PagedData<Order>> GetOrdersByCustomerIdAsync(string userId, OrderQueryParameters queryParameters)
//        {
//            var orders = dbContext.Orders.Where(x => x.AppUserId == userId);

//            var total = await orders.CountAsync();

//            var start = (queryParameters.Page - 1) * queryParameters.Limit;
//            orders = orders.Skip(start).Take(queryParameters.Limit);

//            return new PagedData<Order>(
//                await orders.AsNoTracking().ToListAsync(),
//                queryParameters.Page,
//                queryParameters.Limit,
//                total);
//        }

//        public async Task<int> GetOrderStatusIdByValue(string orderStatus)
//        {
//            var status = await dbContext.OrderStatuses
//                .Where(x => x.Value == orderStatus)
//                .FirstAsync();

//            return status.Id;
//        }

//        public async Task UpdateOrderStatus(Guid orderId, int orderStatusId)
//        {
//            var order = await dbContext.Orders.FindAsync(orderId);

//            if (order == null)
//            {
//                throw new NotFoundException("Cannot find the order.");
//            }

//            order.OrderStatusId = orderStatusId;

//            await dbContext.SaveChangesAsync();
//        }
//    }
//}
