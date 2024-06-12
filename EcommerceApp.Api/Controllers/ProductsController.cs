using AutoMapper;
using EcommerceApp.Api.Dtos;
using EcommerceApp.Api.ModelBinders;
using EcommerceApp.Api.Services.Interfaces;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Api.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;
        private readonly ILoggerService logger;

        public ProductsController(IProductRepository productRepository, IMapper mapper, ILoggerService logger)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int categoryId)
        {
            ICollection<Product> products;
            if (categoryId <= 0)
            {
                products = await productRepository.GetAllAsync();
            }
            else
            {
                products = await productRepository.GetProductsByCategoryAsync(categoryId);
            }

            var productGetDtos = mapper.Map<List<ProductGetDto>>(products);
            return Ok(productGetDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
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
        public async Task<IActionResult> Create([FromBody] ProductPostPutDto productPostPutDto)
        {
            var product = mapper.Map<Product>(productPostPutDto);

            var addedProduct = await productRepository.InsertAsync(product);

            var productGetDto = mapper.Map<ProductGetDto>(addedProduct);

            return CreatedAtAction(nameof(GetById), new { id = addedProduct.Id }, productGetDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductPostPutDto productPostPutDto)
        {
            var updatedProduct = mapper.Map<Product>(productPostPutDto);
            updatedProduct.Id = id;

            var success = await productRepository.UpdateAsync(id, updatedProduct);

            if (!success)
            {
                return NotFound($"Cannot find the product to update.");
            }

            return Ok("Updated the product successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await productRepository.DeleteAsync(id);

            if (!success)
            {
                return NotFound($"Cannot find the product to delete.");
            }

            return NoContent();
        }

        [HttpGet("collection/({ids})")]
        public async Task<IActionResult> GetProductCollection([ModelBinder(BinderType = typeof(ListModelBinder))] IEnumerable<int> ids)
        {
            var products = await productRepository.GetProductsByIdsAsync(ids);

            return Ok(mapper.Map<List<ProductGetDto>>(products));
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateProductList(IEnumerable<ProductPostPutDto> productPostPutDtos)
        {
            var result = new List<ProductGetDto>();
            var products = mapper.Map<IEnumerable<Product>>(productPostPutDtos);
            foreach (var product in products)
            {
                var addedProduct = await productRepository.InsertAsync(product);
                result.Add(mapper.Map<ProductGetDto>(addedProduct));
            }

            return CreatedAtAction(nameof(GetProductCollection), new { ids = string.Join(",", products.Select(p => p.Id)) }, result);
        }
    }
}