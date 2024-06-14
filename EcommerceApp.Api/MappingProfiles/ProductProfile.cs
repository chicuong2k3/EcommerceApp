using AutoMapper;
using EcommerceApp.Api.Dtos;
using EcommerceApp.Domain.Models;
using EcommerceApp.Domain.Shared;

namespace EcommerceApp.Api.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductGetDto>().ReverseMap();
            CreateMap<Product, ProductCreateUpdateDto>().ReverseMap();

            CreateMap<PagingData<Product>, PagingDataDto<ProductGetDto>>().ReverseMap();
        }
    }
}