using AutoMapper;
using EcommerceApp.Api.Dtos.CartDtos;
using EcommerceApp.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Api.Controllers.V1
{
    [ApiController]
    [Route("/api/[controller]")]
    public class CartsController : Controller
    {
        private readonly ICartRepository cartRepository;
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public CartsController(
            ICartRepository cartRepository,
            IProductRepository productRepository,
            IMapper mapper)
        {
            this.cartRepository = cartRepository;
            this.productRepository = productRepository;
            this.mapper = mapper;
        }
        [HttpGet("{appUserId}")]
        public async Task<IActionResult> GetAllCartLines(string appUserId)
        {
            var cartLines = await cartRepository.GetCartLinesAsync(appUserId);
            var result = new List<CartLineDto>();

            foreach (var cartLine in cartLines)
            {
                var productItem = await productRepository.GetProductItemByIdAsync(cartLine.ProductVariant.ProductItemId);

                if (productItem == null)
                {
                    return BadRequest();
                }

                var product = await productRepository.GetByIdAsync(productItem.ProductId);
                
                var cartLineDto = mapper.Map<CartLineDto>(cartLine);

                cartLineDto.Product = mapper.Map<ProductCartDto>(product);
                cartLineDto.Product.Colour = productItem.Colour.Value;

                var productVariant = await productRepository.GetProductVariantByIdAsync(cartLine.ProductVariantId);
                if (productVariant != null)
                    cartLineDto.Product.Size = productVariant.Size.Value;

                result.Add(cartLineDto);
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddProductToCart([FromBody] AddProductToCartDto addProductToCartDto)
        {
            await cartRepository.AddProductsAsync(
                addProductToCartDto.AppUserId,
                addProductToCartDto.ProductVariantId,
                addProductToCartDto.Quantity);

            return Ok("Added product to cart successfully.");
        }

        [HttpDelete("{appUserId}/{productVariationId}")]
        public async Task<IActionResult> RemoveProductFromCart(string appUserId, Guid productVariantId)
        {
            await cartRepository.RemoveProductAsync(appUserId, productVariantId);

            return Ok("Removed product from cart successfully.");
        }
    }
}
