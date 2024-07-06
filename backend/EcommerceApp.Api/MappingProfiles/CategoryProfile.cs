using AutoMapper;
using EcommerceApp.Api.Dtos.CategoryDtos;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Api.MappingProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryGetDto>();
            CreateMap<Category, CategoryCreateUpdateDto>().ReverseMap();
        }
    }
}