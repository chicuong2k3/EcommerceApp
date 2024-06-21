﻿using AutoMapper;
using EcommerceApp.Api.CustomFilters;
using EcommerceApp.Api.Dtos;
using EcommerceApp.Api.ModelBinders;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using EcommerceApp.Domain.Shared;
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

        [HttpGet]
        [HttpHead]
        //[ResponseCache(Duration = 100)]
        [OutputCache(Duration = 100)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> GetAll([FromQuery] ProductQueryParameters queryParameters)
        {
            PagingData<Product> pagingData = await productRepository.GetProductsAsync(queryParameters);

            var pagingDataDto = mapper.Map<PagingDataDto<ProductGetDto>>(pagingData);


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

            var productGetDto = mapper.Map<ProductGetDto>(product);

            return Ok(productGetDto);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Create([FromBody] ProductCreateUpdateDto productCreateUpdateDto)
        {

            var product = mapper.Map<Product>(productCreateUpdateDto);

            var addedProduct = await productRepository.InsertAsync(product);

            var productGetDto = mapper.Map<ProductGetDto>(addedProduct);

            return CreatedAtAction(nameof(GetById), new { id = addedProduct.Id }, productGetDto);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductCreateUpdateDto productCreateUpdateDto)
        {
            var updatedProduct = mapper.Map<Product>(productCreateUpdateDto);
            updatedProduct.Id = id;

            var success = await productRepository.UpdateAsync(updatedProduct);

            if (!success)
            {
                return NotFound($"Cannot find the product to update.");
            }

            return Ok("Updated the product successfully.");
        }

        [HttpDelete("{id}")]
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

            return Ok(mapper.Map<List<ProductGetDto>>(products));
        }

        [HttpPost("collection")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateProductList(IEnumerable<ProductCreateUpdateDto> productCreateUpdateDtos)
        {

            var result = new List<ProductGetDto>();
            var products = mapper.Map<IEnumerable<Product>>(productCreateUpdateDtos);
            foreach (var product in products)
            {
                var addedProduct = await productRepository.InsertAsync(product);
                result.Add(mapper.Map<ProductGetDto>(addedProduct));
            }

            return CreatedAtAction(nameof(GetProductCollection), new { ids = string.Join(",", products.Select(p => p.Id)) }, result);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdate(Guid id, [FromBody] JsonPatchDocument<ProductCreateUpdateDto> patchDocument)
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

            var productCreateUpdateDto = mapper.Map<ProductCreateUpdateDto>(product);

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