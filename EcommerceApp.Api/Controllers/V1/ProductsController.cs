using AutoMapper;
using EcommerceApp.Api.CustomFilters;
using EcommerceApp.Api.Dtos;
using EcommerceApp.Api.ModelBinders;
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
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> GetAll([FromQuery] ProductQueryParameters queryParameters)
        {
            PagingData<Product> pagingData = await productRepository.GetProductsAsync(queryParameters);

            var pagingDataDto = mapper.Map<PagingDataDto<ProductGetDto>>(pagingData);

            pagingDataDto.Items = await BuildProductGetDtoList(pagingData.Items);

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagingDataDto));

            return Ok(pagingDataDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound($"Cannot find the product.");
            }

            return Ok(await BuildProductGetDto(product));
        }

        [HttpPost]
        [Authorize(Roles = UserRoleConstant.Customer)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Create([FromBody] ProductCreateDto productCreateDto)
        {

            var product = mapper.Map<Product>(productCreateDto);

            var optionForColors = mapper.Map<Dictionary<int, List<ProductVariation>>>(productCreateDto.OptionsForColour);

            var addedProduct = await productRepository.CreateAsync(
                product,
                productCreateDto.ColourIds.ToList(),
                productCreateDto.CategoryIds.ToList(),
                optionForColors
                );

            if (addedProduct == null)
            {
                return BadRequest();
            }

            
            return CreatedAtAction(nameof(GetById), new { id = addedProduct.Id }, await BuildProductGetDto(addedProduct));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = UserRoleConstant.Admin)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductUpdateDto productUpdateDto)
        {
            var updatedProduct = mapper.Map<Product>(productUpdateDto);
            updatedProduct.Id = id;

            var success = await productRepository.UpdateAsync(updatedProduct);

            if (!success)
            {
                return NotFound($"Cannot find the product to update.");
            }

            return Ok("Updated the product successfully.");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoleConstant.Admin)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await productRepository.DeleteAsync(id);

            if (!success)
            {
                return NotFound($"Cannot find the product to delete.");
            }

            return NoContent();
        }

        [HttpGet("collection/({ids})")]
        public async Task<IActionResult> GetProductCollection([ModelBinder(BinderType = typeof(ListModelBinder))] IEnumerable<Guid> ids)
        {
            var products = await productRepository.GetProductsByIdsAsync(ids);

            
            return Ok(await BuildProductGetDtoList(products));
        }

        //[HttpPost("collection")]
        //[ServiceFilter(typeof(ValidationFilterAttribute))]
        //public async Task<IActionResult> CreateProductList(IEnumerable<ProductCreateUpdateDto> productCreateUpdateDtos)
        //{

        //    var result = new List<ProductGetDto>();
        //    var products = mapper.Map<IEnumerable<Product>>(productCreateUpdateDtos);
        //    foreach (var product in products)
        //    {
        //        var addedProduct = await productRepository.CreateAsync(product);
        //        result.Add(mapper.Map<ProductGetDto>(addedProduct));
        //    }

        //    return CreatedAtAction(nameof(GetProductCollection), new { ids = string.Join(",", products.Select(p => p.Id)) }, result);
        //}

        [HttpPatch("{id}")]
        [Authorize(Roles = UserRoleConstant.Admin)]
        public async Task<IActionResult> PartiallyUpdate(Guid id, [FromBody] JsonPatchDocument<ProductCreateDto> patchDocument)
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

            var productCreateUpdateDto = mapper.Map<ProductCreateDto>(product);

            patchDocument.ApplyTo(productCreateUpdateDto, ModelState);

            TryValidateModel(productCreateUpdateDto);
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var updatedProduct = mapper.Map<Product>(productCreateUpdateDto);
            updatedProduct.Id = id;

            var success = await productRepository.UpdateAsync(updatedProduct);

            if (!success)
            {
                return NotFound($"Cannot find the product to update.");
            }

            return NoContent();
        }

        [HttpOptions]
        public IActionResult GetProductsOptions()
        {
            Response.Headers.Append("Allow", "GET, OPTIONS, POST, PUT, DELETE, PATCH");
            return Ok();
        }
    }
}