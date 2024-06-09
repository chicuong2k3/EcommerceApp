using AutoMapper;
using EcommerceApp.Api.Dtos;
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

            await productRepository.UpdateAsync(id, updatedProduct);

            return Ok("Updated the product successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await productRepository.DeleteAsync(id);

            return Ok("Deleted the product successfully.");
        }
    }
}