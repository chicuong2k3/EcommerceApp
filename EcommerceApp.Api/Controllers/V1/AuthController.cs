using AutoMapper;
using EcommerceApp.Api.CustomFilters;
using EcommerceApp.Api.Dtos;
using EcommerceApp.Api.Services.Interfaces;
using EcommerceApp.Domain.Models;
using EcommerceApp.Domain.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

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
        private readonly IJwtService jwtService;
        private readonly IMapper mapper;

        public AuthController(UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IJwtService jwtService,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.jwtService = jwtService;
            this.mapper = mapper;
        }

        [HttpPost("")]
        
        public async Task<IActionResult> RegisterUser([FromBody] AppUserCreateDto appUserCreateDto)
        {
            var user = mapper.Map<AppUser>(appUserCreateDto);
            user.RegistrationDate = DateTime.Now;

            var result = await userManager.CreateAsync(user, appUserCreateDto.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }

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

            await userManager.AddToRoleAsync(user, "Customer");
            return Created();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await userManager.FindByNameAsync(loginDto.UserName);

            var isValid = await userManager.CheckPasswordAsync(user, loginDto.Password);

            if (user == null || !isValid)
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
