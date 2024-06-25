using AutoMapper;
using EcommerceApp.Api.Dtos.CartDtos;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Api.MappingProfiles
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<CartItem, CartLineDto>().ReverseMap();
            CreateMap<Product, ProductCartDto>();
        }
    }
}
