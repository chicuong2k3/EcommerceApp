﻿using AutoMapper;
using EcommerceApp.Api.Dtos.CategoryDtos;
using EcommerceApp.Api.Dtos.ProductDtos;
using EcommerceApp.Api.Dtos.SharedDtos;
using EcommerceApp.Common.Shared;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Api.MappingProfiles
{
    public class PaginationProfile : Profile
    {
        public PaginationProfile()
        {
            CreateMap<Pagination, PaginationDto>().ReverseMap();
            CreateMap<PagedData<Category>, PagedDataDto<CategoryGetDto>>().ReverseMap();
            CreateMap<PagedData<Product>, PagedDataDto<ProductGetDto>>().ReverseMap();
            
        }
    }
}
