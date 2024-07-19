using AutoMapper;
using EcommerceApp.Api.CustomFilters;
using EcommerceApp.Api.Dtos.CategoryDtos;
using EcommerceApp.Api.Dtos.ProductDtos;
using EcommerceApp.Api.Dtos.SharedDtos;
using EcommerceApp.Api.ModelBinders;
using EcommerceApp.Common.Shared;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
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
    //[Authorize(Roles = UserRoleConstant.Admin)]
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

        [HttpGet]
        [HttpHead]
        //[ResponseCache(Duration = 100)]
        [OutputCache(Duration = 100)]
        [AllowAnonymous]
        public async Task<IActionResult> ListProducts([FromQuery] ProductQueryParameters queryParameters)
        {
            PagedData<Product> data = await productRepository.GetProductsAsync(queryParameters);

            var dataDto = mapper.Map<PagedDataDto<ProductGetDto>>(data);

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
                return NotFound($"Product with id={id} not found.");
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateUpdateDto productCreateDto)
        {

            var product = mapper.Map<Product>(productCreateDto);

            var addedProduct = await productRepository.InsertAsync(
                product, 
                productCreateDto.CategoryIds);

            var productDto = mapper.Map<ProductGetDto>(addedProduct);

            return CreatedAtAction(nameof(GetProductById), new { id = addedProduct.Id }, productDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductCreateUpdateDto productUpdateDto)
        {
            var updatedProduct = mapper.Map<Product>(productUpdateDto);
            updatedProduct.Id = id;

            //if (string.IsNullOrEmpty(productUpdateDto.ThumbUrl))
            //{
            //    var oldProduct = await productRepository.GetByIdAsync(id);
            //    updatedProduct.ThumbUrl = oldProduct?.ThumbUrl;
            //}
            //else
            //{
            //    updatedProduct.ThumbUrl = productUpdateDto.ThumbUrl;
            //}


            await productRepository.UpdateAsync(updatedProduct, productUpdateDto.CategoryIds);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            await productRepository.DeleteAsync(id);

            return NoContent();
        }

        //[HttpGet("collection/({ids})")]
        //public async Task<IActionResult> GetProductCollection([ModelBinder(BinderType = typeof(ListModelBinder))] IEnumerable<Guid> ids)
        //{
        //    var products = await productRepository.GetProductsByIdsAsync(ids);
        //    return Ok(await BuildProductGetDtoList(products));
        //}

        //[HttpPatch("{id}")]
        //public async Task<IActionResult> PartiallyUpdateProduct(Guid id, [FromBody] JsonPatchDocument<ProductCreateUpdateDto> patchDocument)
        //{
        //    if (patchDocument == null)
        //    {
        //        return BadRequest();
        //    }

        //    var product = await productRepository.GetByIdAsync(id);

        //    if (product == null)
        //    {
        //        return NotFound($"Product with id={id} not found.");
        //    }

        //    var productUpdateDto = mapper.Map<ProductCreateUpdateDto>(product);

        //    patchDocument.ApplyTo(productUpdateDto, ModelState);

        //    TryValidateModel(productUpdateDto);
        //    if (!ModelState.IsValid)
        //    {
        //        return UnprocessableEntity(ModelState);
        //    }

        //    var updatedProduct = mapper.Map<Product>(productUpdateDto);
        //    updatedProduct.Id = id;

        //    await productRepository.UpdateAsync(updatedProduct);

        //    return NoContent();
        //}

        [HttpOptions]
        public IActionResult GetProductsOptions()
        {
            Response.Headers.Append("Allow", "GET, OPTIONS, POST, PUT, DELETE, PATCH");
            return Ok();
        }

       

    }
}