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
            CreateMap<Product, ProductCreateDto>().ReverseMap();
            CreateMap<Product, ProductUpdateDto>().ReverseMap();

            CreateMap<PagingData<Product>, PagingDataDto<ProductGetDto>>().ReverseMap();


            CreateMap<ProductVariation, ProductVariationDto>().ReverseMap();
        }
    }
}