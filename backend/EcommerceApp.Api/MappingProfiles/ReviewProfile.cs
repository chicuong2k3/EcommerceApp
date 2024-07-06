using AutoMapper;
using EcommerceApp.Api.Dtos.ReviewDtos;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Api.MappingProfiles
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<Review, ReviewGetDto>();
            CreateMap<Review, ReviewCreateDto>().ReverseMap();
            CreateMap<Review, ReviewUpdateDto>().ReverseMap();
        }
    }
}
