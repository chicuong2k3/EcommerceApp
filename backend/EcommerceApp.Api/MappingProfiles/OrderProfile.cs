using AutoMapper;
using EcommerceApp.Api.Dtos.OrderDtos;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Api.MappingProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderGetDto>();
            CreateMap<OrderLine, OrderLineGetDto>();
            CreateMap<Order, CreateOrderDto>().ReverseMap();
            CreateMap<OrderLine, OrderDetail>().ReverseMap();
        }
    }
}
