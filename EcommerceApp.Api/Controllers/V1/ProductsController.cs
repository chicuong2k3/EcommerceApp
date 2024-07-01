using AutoMapper;
using EcommerceApp.Api.CustomFilters;
using EcommerceApp.Api.Dtos.CategoryDtos;
using EcommerceApp.Api.Dtos.ProductDtos;
using EcommerceApp.Api.Dtos.SharedDtos;
using EcommerceApp.Api.ModelBinders;
using EcommerceApp.Domain.Constants;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using EcommerceApp.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using System.Text.Json;

namespace EcommerceApp.Api.Controllers.V1
{
    [ApiController]
    [Route("/api/[controller]")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [Authorize(Roles = UserRoleConstant.Admin)]
    //[ResponseCache(CacheProfileName = "ExpireIn300s")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;
        private readonly ILogger<ProductsController> logger;

        public ProductsController(IProductRepository productRepository,
            IMapper mapper,
            ILogger<ProductsController> logger)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        private async Task<ProductGetDto> BuildProductGetDto(Product product)
        {
            var productGetDto = mapper.Map<ProductGetDto>(product);

            var categories = await productRepository.GetCategoriesOfProductAsync(product.Id);
            productGetDto.Categories = mapper.Map<List<CategoryGetDto>>(categories);

            var colours = await productRepository.GetColoursOfProductAsync(product.Id);
            productGetDto.Colours = mapper.Map<List<ColourGetDto>>(colours);

            return productGetDto;
        }

        private async Task<List<ProductGetDto>> BuildProductGetDtoList(List<Product> products)
        {
            var productGetDtos = mapper.Map<List<ProductGetDto>>(products);

            foreach (var item in productGetDtos)
            {

                var categories = await productRepository.GetCategoriesOfProductAsync(item.Id);
                item.Categories = mapper.Map<List<CategoryGetDto>>(categories);

                var colours = await productRepository.GetColoursOfProductAsync(item.Id);
                item.Colours = mapper.Map<List<ColourGetDto>>(colours);
            }

            return productGetDtos;
        }

        [HttpGet]
        [HttpHead]
        //[ResponseCache(Duration = 100)]
        [OutputCache(Duration = 100)]
        [AllowAnonymous]
        public async Task<IActionResult> ListProducts([FromQuery] ProductQueryParameters queryParameters)
        {
            PagedData<Product> data = await productRepository.GetProductsAsync(queryParameters);

            var dataDto = mapper.Map<PagedDataDto<ProductGetDto>>(data);

            dataDto.Items = await BuildProductGetDtoList(data.Items);

            //Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(data));

            return Ok(new { data = dataDto.Items, pagination = dataDto.Pagination });
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound($"Cannot find the product.");
            }

            return Ok(await BuildProductGetDto(product));
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto productCreateDto)
        {

            var product = mapper.Map<Product>(productCreateDto);
            var addedProduct = await productRepository.InsertAsync(product, 
                productCreateDto.CategoryIds.ToList());

            if (addedProduct == null)
            {
                return StatusCode(500);
            }

            return CreatedAtAction(nameof(GetProductById), new { id = addedProduct.Id }, await BuildProductGetDto(addedProduct));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductUpdateDto productUpdateDto)
        {

            var existingProduct = await productRepository.GetByIdAsync(id);

            var updatedProduct = mapper.Map<Product>(productUpdateDto);
            updatedProduct.Id = id;

            if (string.IsNullOrEmpty(productUpdateDto.ThumbUrl))
            {
                var oldProduct = await productRepository.GetByIdAsync(id);
                updatedProduct.ThumbUrl = oldProduct?.ThumbUrl;
            }
            else
            {

            }


            await productRepository.UpdateAsync(updatedProduct);

            if (existingProduct == null)
            {
                return CreatedAtAction(nameof(GetProductById), new { id }, updatedProduct);
            }

            return Ok("Updated the product successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            await productRepository.DeleteAsync(id);

            return NoContent();
        }

        [HttpGet("collection/({ids})")]
        public async Task<IActionResult> GetProductCollection([ModelBinder(BinderType = typeof(ListModelBinder))] IEnumerable<Guid> ids)
        {
            var products = await productRepository.GetProductsByIdsAsync(ids);
            return Ok(await BuildProductGetDtoList(products));
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateProduct(Guid id, [FromBody] JsonPatchDocument<ProductUpdateDto> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var product = await productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound($"Cannot find the product to update.");
            }

            var productUpdateDto = mapper.Map<ProductUpdateDto>(product);

            patchDocument.ApplyTo(productUpdateDto, ModelState);

            TryValidateModel(productUpdateDto);
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var updatedProduct = mapper.Map<Product>(productUpdateDto);
            updatedProduct.Id = id;

            await productRepository.UpdateAsync(updatedProduct);

            return NoContent();
        }

        [HttpOptions]
        public IActionResult GetProductsOptions()
        {
            Response.Headers.Append("Allow", "GET, OPTIONS, POST, PUT, DELETE, PATCH");
            return Ok();
        }

        [HttpGet("{productId}/variants/{variantNumber}")]
        public async Task<IActionResult> GetProductVariant(Guid productId, int variantNumber)
        {
            var productVariant = await productRepository.GetProductVariantAsync(productId, variantNumber);

            if (productVariant == null)
            {
                return NotFound("Cannot find the product variant.");
            }

            var variantDto = mapper.Map<ProductVariantGetDto>(productVariant);

            return Ok(variantDto);

        }

        [HttpGet("{productId}/variants")]
        public async Task<IActionResult> GetProductVariants(Guid productId)
        {
            var productVariants = await productRepository.GetProductVariantsAsync(productId);

            var result = mapper.Map<List<ProductVariantGetDto>>(productVariants);

            return Ok(result);

        }

        [HttpPost("{productId}/variants")]
        public async Task<IActionResult> AddVariantForProduct(ProductVariantAddDto productVariantAddDto)
        {
            var variant = mapper.Map<ProductVariant>(productVariantAddDto);

            var addedVariant = await productRepository.AddVariantForProductAsync(productVariantAddDto.ProductId, variant);

            if (addedVariant == null)
            {
                return StatusCode(500);
            }

            var variantGetDto = mapper.Map<ProductVariantGetDto>(productVariantAddDto);

            return CreatedAtAction(nameof(GetProductVariant), new { }, variantGetDto);

        }

    }
}