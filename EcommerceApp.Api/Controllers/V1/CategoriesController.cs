using Asp.Versioning;
using AutoMapper;
using EcommerceApp.Api.CustomFilters;
using EcommerceApp.Api.Dtos;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace EcommerceApp.Api.Controllers.V1
{
    [ApiVersion("1.0", Deprecated = true)]
    [ApiController]
    [Route("/api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;

        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        //[OutputCache(NoStore = true)]
        // response will be cached until we change key
        //[OutputCache(VaryByQueryKeys = new[] { nameof(key) }, Duration = 30)] 
        [EnableRateLimiting("3RequestPer30SecondsRateLimit")]
        public async Task<IActionResult> GetAll()
        {
            var categories = await categoryRepository.GetCategoriesAsync();

            var data = mapper.Map<List<CategoryGetDto>>(categories);

            foreach (var categoryGetDto in data)
            {
                var parentCategory = await categoryRepository.GetParentCategoryOfAsync(categoryGetDto.Id);

                if (parentCategory != null)
                {
                    categoryGetDto.ParentCategory = mapper.Map<ParentCategoryDto>(parentCategory);
                }
            }

            return Ok(data);
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(int id)
        {
            var category = await categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound($"Cannot find the category.");
            }
            

            var categoryGetDto = mapper.Map<CategoryGetDto>(category);
            var parentCategory = await categoryRepository.GetParentCategoryOfAsync(category.Id);

            if (parentCategory != null)
            {
                categoryGetDto.ParentCategory = mapper.Map<ParentCategoryDto>(parentCategory);
            }

            return Ok(categoryGetDto);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Create([FromBody] CategoryCreateUpdateDto categoryCreateUpdateDto)
        {

            var category = mapper.Map<Category>(categoryCreateUpdateDto);

            var addedCategory = await categoryRepository.InsertAsync(category);

            if (addedCategory == null)
            {
                return StatusCode(500);
            }

            var categoryGetDto = mapper.Map<CategoryGetDto>(addedCategory);

            var parentCategory = await categoryRepository.GetParentCategoryOfAsync(category.Id);

            if (parentCategory != null)
            {
                categoryGetDto.ParentCategory = mapper.Map<ParentCategoryDto>(parentCategory);
            }

            return CreatedAtAction(nameof(GetById), new { id = addedCategory.Id }, categoryGetDto);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryCreateUpdateDto categoryCreateUpdateDto)
        {
            var category = await categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound($"Cannot find the category to update.");
            }

            var updatedCategory = mapper.Map<Category>(categoryCreateUpdateDto);
            updatedCategory.Id = id;

            await categoryRepository.UpdateAsync(updatedCategory);

            return Ok("Updated the category successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await categoryRepository.DeleteAsync(id);

            if (!success)
            {
                return NotFound($"Cannot find the category to delete.");
            }

            return NoContent();
        }
    }
}