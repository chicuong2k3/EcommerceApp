using Asp.Versioning;
using AutoMapper;
using EcommerceApp.Api.CustomFilters;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using EcommerceApp.Domain.Shared;
using EcommerceApp.Api.Dtos.CategoryDtos;
using EcommerceApp.Api.Dtos.SharedDtos;
using Microsoft.AspNetCore.Authorization;
using EcommerceApp.Domain.Constants;

namespace EcommerceApp.Api.Controllers.V1
{
    //[ApiVersion("1.0", Deprecated = false)]
    [ApiController]
    [Route("/api/[controller]")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [Authorize(Roles = UserRoleConstant.Admin)]
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
        [AllowAnonymous]
        public async Task<IActionResult> ListCategories([FromQuery] CategoryQueryParameters queryParameters)
        {
            var data = await categoryRepository.GetCategoriesAsync(queryParameters);

            var dataDto = mapper.Map<PagedDataDto<CategoryGetDto>>(data);

            return Ok(new { data = dataDto.Items, pagination = dataDto.Pagination });
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound($"Cannot find the category.");
            }

            var categoryGetDto = mapper.Map<CategoryGetDto>(category);
            return Ok(categoryGetDto);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateUpdateDto categoryCreateUpdateDto)
        {
           
            var category = mapper.Map<Category>(categoryCreateUpdateDto);
            category.Slug = category.Name.GenerateSlug();

            var addedCategory = await categoryRepository.InsertAsync(category);

            if (addedCategory == null)
            {
                return StatusCode(500);
            }

            var categoryGetDto = mapper.Map<CategoryGetDto>(addedCategory);

            return CreatedAtAction(nameof(GetCategoryById), new { id = addedCategory.Id }, categoryGetDto);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryCreateUpdateDto categoryCreateUpdateDto)
        {
            var updatedCategory = mapper.Map<Category>(categoryCreateUpdateDto);
            updatedCategory.Id = id;

            await categoryRepository.UpdateAsync(updatedCategory);

            return Ok("Updated the category successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await categoryRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}