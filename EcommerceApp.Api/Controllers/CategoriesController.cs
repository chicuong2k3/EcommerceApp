﻿using AutoMapper;
using EcommerceApp.Api.Dtos;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Api.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;

        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await categoryRepository.GetAllAsync();

            var categoryGetDtos = mapper.Map<List<CategoryGetDto>>(categories);

            return Ok(categoryGetDtos);
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

            return Ok(categoryGetDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryPostPutDto categoryPostPutDto)
        {
            var category = mapper.Map<Category>(categoryPostPutDto);

            var addedCategory = await categoryRepository.InsertAsync(category);

            var categoryGetDto = mapper.Map<CategoryGetDto>(addedCategory);

            return CreatedAtAction(nameof(GetById), new { id = addedCategory.Id }, categoryGetDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryPostPutDto categoryPostPutDto)
        {
            var updatedCategory = mapper.Map<Category>(categoryPostPutDto);
            updatedCategory.Id = id;

            await categoryRepository.UpdateAsync(id, updatedCategory);

            return Ok("Updated the category successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await categoryRepository.DeleteAsync(id);

            return Ok("Deleted the category successfully.");
        }
    }
}