using AutoMapper;
using EcommerceApp.Api.CustomFilters;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using EcommerceApp.Domain.Shared;
using EcommerceApp.Api.Dtos.SharedDtos;
using Microsoft.AspNetCore.Authorization;
using EcommerceApp.Api.Dtos.ReviewDtos;

namespace EcommerceApp.Api.Controllers.V1
{
    [ApiController]
    [Route("/api/[controller]")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [Authorize]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewRepository reviewRepository;
        private readonly IMapper mapper;

        public ReviewsController(IReviewRepository reviewRepository, IMapper mapper)
        {
            this.reviewRepository = reviewRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ListReviews([FromQuery] ReviewQueryParameters queryParameters)
        {
            var data = await reviewRepository.GetReviewsAsync(queryParameters);

            var dataDto = mapper.Map<PagedDataDto<ReviewGetDto>>(data);

            return Ok(new { data = dataDto.Items, pagination = dataDto.Pagination });
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetReviewById(Guid id)
        {
            var review = await reviewRepository.GetByIdAsync(id);

            if (review == null)
            {
                return NotFound($"Cannot find the review.");
            }

            var dto = mapper.Map<ReviewGetDto>(review);
            return Ok(dto);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateReview([FromBody] ReviewCreateDto reviewCreateDto)
        {
           
            var review = mapper.Map<Review>(reviewCreateDto);

            var addedReview = await reviewRepository.InsertAsync(review);

            if (addedReview == null)
            {
                return StatusCode(500);
            }

            var getDto = mapper.Map<ReviewGetDto>(addedReview);

            return CreatedAtAction(nameof(GetReviewById), new { id = addedReview.Id }, getDto);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateReview(Guid id, [FromBody] ReviewUpdateDto reviewUpdateDto)
        {
            var updatedReview = mapper.Map<Review>(reviewUpdateDto);
            updatedReview.Id = id;

            await reviewRepository.UpdateAsync(updatedReview);

            return Ok("Updated the review successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(Guid id)
        {
            await reviewRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}