using AutoMapper;
using EcommerceApp.Api.Dtos;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Api.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductGetDto>()
                .ReverseMap();
            CreateMap<Product, ProductCreateUpdateDto>().ReverseMap();
        }
    }
}