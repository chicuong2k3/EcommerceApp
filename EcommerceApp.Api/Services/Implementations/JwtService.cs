using EcommerceApp.Api.Constants;
using EcommerceApp.Api.Dtos;
using EcommerceApp.Api.Services.Interfaces;
using EcommerceApp.Api.Settings;
using EcommerceApp.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EcommerceApp.Api.Services.Implementations
{
    public class JwtService : IJwtService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IConfiguration configuration;
        private readonly JwtSettings jwtSettings;

        public JwtService(UserManager<AppUser> userManager, 
            IConfiguration configuration,
            IOptionsMonitor<JwtSettings> jwtSettingsOptions)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.jwtSettings  = jwtSettingsOptions.CurrentValue;
        }

        private string CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(randomNumber);
            }
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> GenerateAccessTokenAsync(AppUser user)
        {
            var secretKey = Environment.GetEnvironmentVariable(EnviromentVariableConstant.SECRET);

            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentNullException("Secret Key does not exist.");
            }

            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty)
            };

            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenOptions = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.Expires)),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            
        }

        private ClaimsPrincipal GetPrincipalFromAccessToken(string accessToken)
        {
            var secretKey = Environment.GetEnvironmentVariable(EnviromentVariableConstant.SECRET);

            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentNullException($"{nameof(JwtService)}: Secret Key does not exist.");
            }

            var tokenValidationParams = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principle = tokenHandler.ValidateToken(accessToken, tokenValidationParams, out var securityToken);

            var jwt = securityToken as JwtSecurityToken;
            if (jwt == null || !jwt.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid access token.");
            }

            return principle;
        }
        public async Task<TokenDto> CreateTokenAsync(AppUser user, bool populateExpire)
        {
            var accessToken = await GenerateAccessTokenAsync(user);
            var refreshToken = CreateRefreshToken();

            user.RefreshToken = refreshToken;

            if (populateExpire)
            {
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(14);
            }

            await userManager.UpdateAsync(user);

            return new TokenDto()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiryTime = user.RefreshTokenExpiryTime
            };
        }

        public async Task<TokenDto?> RefreshTokenAsync(TokenDto tokenDto)
        {
            var principal = GetPrincipalFromAccessToken(tokenDto.AccessToken);

            var user = await userManager.FindByNameAsync(principal.Identity.Name);
            if (user == null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return null;

            return await CreateTokenAsync(user, false);
        }
    }
}
