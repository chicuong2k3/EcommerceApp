using AutoMapper;
using EcommerceApp.Api.CustomFilters;
using EcommerceApp.Api.Dtos.AuthenticationDtos;
using EcommerceApp.Api.Services.Interfaces;
using EcommerceApp.Common.Constants;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Data;

namespace EcommerceApp.Api.Controllers.V1
{
    [ApiController]
    [Route("/api/[controller]")]
    [EnableRateLimiting("AuthenticationRateLimit")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ICartRepository cartRepository;
        private readonly IJwtService jwtService;
        private readonly IMapper mapper;

        public AuthController(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ICartRepository cartRepository,
            IJwtService jwtService,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.cartRepository = cartRepository;
            this.jwtService = jwtService;
            this.mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] AppUserCreateDto appUserCreateDto)
        {
            var user = mapper.Map<AppUser>(appUserCreateDto);
            user.RegistrationDate = DateTime.Now;

            if (!await roleManager.RoleExistsAsync(UserRoleConstant.Customer))
            {
                var roleCreationResult = await roleManager.CreateAsync(new IdentityRole()
                {
                    Name = UserRoleConstant.Customer,
                    NormalizedName = UserRoleConstant.NormalizedCustomer
                });

                if (!roleCreationResult.Succeeded)
                {
                    return StatusCode(500);
                }
            }

            var result = await userManager.CreateAsync(user, appUserCreateDto.Password);

            if (!result.Succeeded)
            {
                return StatusCode(500);
            }

            var addResult = await userManager.AddToRoleAsync(user, UserRoleConstant.Customer);

            if (!addResult.Succeeded)
            {
                await userManager.DeleteAsync(user);
                return StatusCode(500);
            }

            var cart = await cartRepository.CreateCartAsync(new Cart() 
            { 
                AppUserId = user.Id 
            });

            if (cart == null)
            {
                await userManager.DeleteAsync(user);
                return StatusCode(500);
            }

            return Created();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await userManager.FindByNameAsync(loginDto.UserName);

            if (user == null)
            {
                return BadRequest("Login information is wrong.");
            }

            var isValid = await userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!isValid)
            {
                return BadRequest("Login information is wrong.");
            }

            var tokenDto = await jwtService.CreateTokenAsync(user, true);
            return Ok(tokenDto);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenDto tokenDto) 
        {
            var newTokenDto = await jwtService.RefreshTokenAsync(tokenDto);

            if (newTokenDto == null) 
                return BadRequest("The token is not valid.");

            return Ok(newTokenDto);
        }
    }
}
