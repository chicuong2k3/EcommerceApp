using AutoMapper;
using EcommerceApp.Api.CustomFilters;
using EcommerceApp.Api.Dtos.CartDtos;
using EcommerceApp.Api.Dtos.SharedDtos;
using EcommerceApp.Common.Constants;
using EcommerceApp.Common.Shared;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcommerceApp.Api.Controllers.V1
{
    [ApiController]
    [Route("/api/[controller]")]
    [Authorize(Roles = UserRoleConstant.Customer)]
    [ServiceFilter(typeof(ValidateCartOwnerFilterAttribute))]
    public class CartsController : Controller
    {
        private readonly ICartRepository cartRepository;
        private readonly IProductRepository productRepository;
        private readonly UserManager<AppUser> userManager;
        private readonly IMapper mapper;

        public CartsController(
            ICartRepository cartRepository,
            IProductRepository productRepository,
            UserManager<AppUser> userManager,
            IMapper mapper)
        {
            this.cartRepository = cartRepository;
            this.productRepository = productRepository;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get the cart of the current authenticated user
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Return the cart of the current authenticated user</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="404">The user has no cart</response>
        /// <response code="422">The request is not valid</response>
        [HttpGet]
        [SkipCartOwnerCheck]
        public async Task<IActionResult> GetCartOfCurrentCustomer()
        {
            var user = await userManager.FindByNameAsync(User.FindFirstValue(ClaimTypes.Name) ?? string.Empty);

            if (user == null)
            {
                return Unauthorized();
            }

            var cart = await cartRepository.GetCartByOwnerIdAsync(user.Id);

            if (cart == null) return NotFound();

            var cartDto = mapper.Map<CartGetDto>(cart);

            return Ok(cartDto);
        }

        [HttpGet("{cartId}/items")]
        public async Task<IActionResult> ListCartItems(string cartId, [FromQuery] CartItemQueryParameters queryParameters)
        {
            var cartItems = await cartRepository.GetCartItemsAsync(cartId, queryParameters);
            var result = mapper.Map<PagedDataDto<CartItemGetDto>>(cartItems);

            return Ok(new
            {
                data = result.Items,
                pagination = result.Pagination
            });
        }

        [HttpGet("{cartId}/items/{cartItemId}")]
        public async Task<IActionResult> GetCartItemById(string cartId, string cartItemId)
        {
            var cartItem = await cartRepository.GetCartItemByIdAsync(cartItemId);

            if (cartItem == null) return NotFound("The cart item is not found.");

            return Ok(mapper.Map<CartItemGetDto>(cartItem));
        }

        [HttpPost("{cartId}/items")]
        public async Task<IActionResult> AddProductToCart(string cartId, [FromBody] AddProductToCartDto addProductToCartDto)
        {

            var cartItem = await cartRepository.AddProductsAsync(
                cartId,
                addProductToCartDto.ProductItemId,
                addProductToCartDto.Quantity);

            var cartItemDto = mapper.Map<CartItemGetDto>(cartItem);

            return CreatedAtAction(nameof(GetCartItemById), new { cartId = cartItem.CartId, cartItemId = cartItem.Id }, cartItemDto);
        }

        [HttpDelete("{cartId}/items/{cartItemId}")]
        public async Task<IActionResult> DecreaseProductQuantity(string cartId, string cartItemId)
        {

            await cartRepository.DecreaseProductQuantityAsync(cartItemId, 1);

            return NoContent();
        }

        [HttpDelete("{cartId}/items")]
        public async Task<IActionResult> ClearCart(string cartId)
        {
            await cartRepository.ClearCartAsync(cartId);

            return NoContent();
        }
    }
}
