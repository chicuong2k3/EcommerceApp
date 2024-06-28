using AutoMapper;
using EcommerceApp.Api.Dtos.OrderDtos;
using EcommerceApp.Api.Dtos.SharedDtos;
using EcommerceApp.Domain.Constants;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using EcommerceApp.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EcommerceApp.Api.Controllers.V1
{
    [ApiController]
    [Route("/api/[controller]")]
    [Authorize(Roles = UserRoleConstant.Customer)]
    public class OrdersController : Controller
    {
        private readonly IOrderRepository orderRepository;
        private readonly UserManager<AppUser> userManager;
        private readonly IMapper mapper;

        public OrdersController(
            IOrderRepository orderRepository,
            UserManager<AppUser> userManager,
            IMapper mapper)
        {
            this.orderRepository = orderRepository;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> ListOrdersOfCurrentCustomer(OrderQueryParameters queryParameters)
        {
            var user = await userManager.FindByNameAsync(User.FindFirstValue(ClaimTypes.Name) ?? string.Empty);

            if (user == null)
            {
                return Unauthorized();
            }

            var orders = await orderRepository.GetOrdersByCustomerIdAsync(user.Id, queryParameters);

            var dto = mapper.Map<PagedDataDto<OrderGetDto>>(orders);

            return Ok(new { data = dto.Items, pagination = dto.Pagination });
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(Guid orderId)
        {
            var order = await orderRepository.GetByIdAsync(orderId);

            if (order == null) return NotFound("The order is not found.");

            return Ok(mapper.Map<OrderGetDto>(order));
        }


        [HttpGet("{orderId}/items")]
        public async Task<IActionResult> GetOrderLines(Guid orderId, [FromQuery] OrderLineQueryParameters queryParameters)
        {
            var orderLines = await orderRepository.GetOrderLinesAsync(orderId, queryParameters);
            var result = mapper.Map<PagedDataDto<OrderLineGetDto>>(orderLines);

            return Ok(new
            {
                data = result.Items,
                pagination = result.Pagination
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            var order = mapper.Map<Order>(createOrderDto);
            var orderLines = mapper.Map<List<OrderLine>>(createOrderDto.OrderDetails);
            var addedOrder = await orderRepository.CreateAsync(order, orderLines);

            if (addedOrder == null)
            {
                return StatusCode(500);
            }

            var dto = mapper.Map<OrderGetDto>(addedOrder);

            return CreatedAtAction(nameof(GetOrderById), new { orderId = addedOrder.Id }, dto);
        }

        [HttpPost("{orderId}/actions/cancel")]
        public async Task<IActionResult> CancelOrder(Guid orderId)
        {
            var cancelStatusId = await orderRepository.GetOrderStatusIdByValue(OrderStatusConstants.Cancelled);
            await orderRepository.UpdateOrderStatus(orderId, cancelStatusId);

            return Ok("Cancel the order successfully.");
        }

        [HttpPost("{orderId}/actions/confirm")]
        public async Task<IActionResult> ConfirmSuccessDelivery(Guid orderId)
        {

            var successStatusId = await orderRepository.GetOrderStatusIdByValue(OrderStatusConstants.Completed);
            await orderRepository.UpdateOrderStatus(orderId, successStatusId);

            return Ok();
        }
    }
}
