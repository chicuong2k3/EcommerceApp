using AutoMapper;
using EcommerceApp.Api.Dtos;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Api.MappingProfiles
{
    public class ColourProfile : Profile
    {
        public ColourProfile()
        {
            CreateMap<Colour, ColourGetDto>()
                .ForMember(dest => dest.ColourCode, config => config.MapFrom(src => src.Value))
                .ReverseMap();
        }
    }
}
