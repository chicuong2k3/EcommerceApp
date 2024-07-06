using AutoMapper;
using EcommerceApp.Api.Dtos.ProductDtos;
using EcommerceApp.Domain.Models;
using EcommerceApp.Domain.Shared;

namespace EcommerceApp.Api.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductGetDto>();
            CreateMap<Product, ProductCreateDto>().ReverseMap();
            CreateMap<Product, ProductUpdateDto>().ReverseMap();

            CreateMap<ProductVariant, ProductVariantGetDto>();

            CreateMap<ProductVariant, ProductVariantAddDto>().ReverseMap();
        }
    }
}