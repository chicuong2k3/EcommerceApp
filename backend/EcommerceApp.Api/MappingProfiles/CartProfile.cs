using AutoMapper;
using EcommerceApp.Api.Dtos.CartDtos;
using EcommerceApp.Api.Dtos.SharedDtos;
using EcommerceApp.Domain.Models;
using EcommerceApp.Domain.Shared;

namespace EcommerceApp.Api.MappingProfiles
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<CartItem, CartItemGetDto>()
                .ForMember(dest => dest.CartItemId, config => config.MapFrom(src => src.Id));
            CreateMap<Product, CartItemGetDto>();
            CreateMap<PagedData<CartItem>, PagedDataDto<CartItemGetDto>>();

            CreateMap<Cart, CartGetDto>()
                .ForMember(dest => dest.CartId, config => config.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, config => config.MapFrom(src => src.AppUserId));
        }
    }
}
