﻿using AutoMapper;
using EcommerceApp.Api.Dtos;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Api.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<AppUser, AppUserCreateDto>().ReverseMap();
        }
    }
}
