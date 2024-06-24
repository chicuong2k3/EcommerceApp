﻿using EcommerceApp.Api.Dtos;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Api.Services.Interfaces
{
    public interface IJwtService
    {
        Task<TokenDto?> RefreshTokenAsync(TokenDto tokenDto);
        Task<TokenDto> CreateTokenAsync(AppUser user, bool populateExpire);
    }
}
