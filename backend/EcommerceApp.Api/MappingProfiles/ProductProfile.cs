using AutoMapper;
using EcommerceApp.Api.Dtos.ProductDtos;


namespace EcommerceApp.Api.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductGetDto>();
            CreateMap<Product, ProductCreateUpdateDto>().ReverseMap();
            CreateMap<Product, ProductCreateUpdateDto>().ReverseMap();

            CreateMap<ProductItem, ProductItemDto>().ReverseMap();
            CreateMap<ProductVariation, ProductVariationDto>().ReverseMap();
        }
    }
}