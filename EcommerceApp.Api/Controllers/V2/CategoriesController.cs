﻿//using Asp.Versioning;
//using AutoMapper;
//using EcommerceApp.Api.CustomFilters;
//using EcommerceApp.Api.Dtos;
//using EcommerceApp.Domain.Interfaces;
//using EcommerceApp.Domain.Models;
//using Microsoft.AspNetCore.Mvc;

//namespace EcommerceApp.Api.Controllers.V2
//{
//    [ApiVersion("2.0")]
//    [ApiController]
//    [Route("/api/[controller]")]
//    //[Route("/api/{v:apiversion}/[controller]")]
//    public class CategoriesController : ControllerBase
//    {
//        private readonly ICategoryRepository categoryRepository;
//        private readonly IMapper mapper;

//        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
//        {
//            this.categoryRepository = categoryRepository;
//            this.mapper = mapper;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAll()
//        {
//            var categories = await categoryRepository.GetCategoriesAsync();

//            var data = mapper.Map<List<CategoryGetDto>>(categories);

//            var result = data.Select(x => x.Name);

//            return Ok(result);
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetById(int id)
//        {
//            var category = await categoryRepository.GetByIdAsync(id);

//            if (category == null)
//            {
//                return NotFound($"Cannot find the category.");
//            }

//            var categoryGetDto = mapper.Map<CategoryGetDto>(category);

//            return Ok(categoryGetDto);
//        }

//        [HttpPost]
//        [ServiceFilter(typeof(ValidationFilterAttribute))]
//        public async Task<IActionResult> Create([FromBody] CategoryCreateUpdateDto categoryCreateUpdateDto)
//        {

//            var category = mapper.Map<Category>(categoryCreateUpdateDto);

//            var addedCategory = await categoryRepository.InsertAsync(category);

//            var categoryGetDto = mapper.Map<CategoryGetDto>(addedCategory);

//            return CreatedAtAction(nameof(GetById), new { id = addedCategory.Id }, categoryGetDto);
//        }

//        [HttpPut("{id}")]
//        [ServiceFilter(typeof(ValidationFilterAttribute))]
//        public async Task<IActionResult> Update(int id, [FromBody] CategoryCreateUpdateDto categoryCreateUpdateDto)
//        {
//            var category = await categoryRepository.GetByIdAsync(id);

//            if (category == null)
//            {
//                return NotFound($"Cannot find the category to update.");
//            }

//            var updatedCategory = mapper.Map<Category>(categoryCreateUpdateDto);
//            updatedCategory.Id = id;

//            await categoryRepository.UpdateAsync(updatedCategory);

//            return Ok("Updated the category successfully.");
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var success = await categoryRepository.DeleteAsync(id);

//            if (!success)
//            {
//                return NotFound($"Cannot find the category to delete.");
//            }

//            return NoContent();
//        }
//    }
//}