using Asp.Versioning;
using EcommerceApp.Api.Constants;
using EcommerceApp.Api.Services.Implementations;
using EcommerceApp.Api.Services.Interfaces;
using EcommerceApp.Api.Settings;
using EcommerceApp.DAL;
using EcommerceApp.DAL.Repositories;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;

namespace EcommerceApp.Api.ExtensionMethods
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            return services;
        }

        public static IServiceCollection AddApiVersioningConfiguration(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ApiVersionReader = new HeaderApiVersionReader("api-version"); // versioning using HTTP header
                // options.ApiVersionReader = new QueryStringApiVersionReader("api-version"); // versioning using query string
            }).AddMvc();
            //.AddMvc(options =>
            // {
            //     options.Conventions.Controller<EcommerceApp.Api.Controllers.V1.CategoriesController>()
            //     .HasDeprecatedApiVersion(new ApiVersion(1, 0));
            //     options.Conventions.Controller<EcommerceApp.Api.Controllers.V2.CategoriesController>()
            //     .HasApiVersion(new ApiVersion(2, 0));
            // });


            return services;
        }

        public static IServiceCollection AddCaching(this IServiceCollection services)
        {
            services.AddResponseCaching();
            services.AddOutputCache(options =>
            {
                options.AddBasePolicy(policy =>
                {
                    policy.Expire(TimeSpan.FromSeconds(5)); // this policy doesn't affect OutputCache attribute
                });

                options.AddPolicy("ExpireIn30s", policy =>
                {
                    policy.Expire(TimeSpan.FromSeconds(30));
                });
            });
            return services;
        }

        public static IServiceCollection AddRateLimiting(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                {
                    return RateLimitPartition.GetFixedWindowLimiter("GlobalLimiter", partition =>
                    {
                        return new FixedWindowRateLimiterOptions()
                        {
                            AutoReplenishment = true,
                            PermitLimit = 5,
                            QueueLimit = 2,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            Window = TimeSpan.FromMinutes(1)
                        };
                    });
                });

                options.AddPolicy("3RequestPer30SecondsRateLimit", context =>
                {
                    return RateLimitPartition.GetFixedWindowLimiter("3RequestPer30SecondsRateLimit", partition =>
                    {
                        return new FixedWindowRateLimiterOptions()
                        {
                            AutoReplenishment = true,
                            PermitLimit = 3,
                            QueueLimit = 0,
                            Window = TimeSpan.FromSeconds(30)
                        };
                    });
                });

                options.AddPolicy("AuthenticationRateLimit", context =>
                {
                    return RateLimitPartition.GetFixedWindowLimiter("AuthenticationRateLimit", partition =>
                    {
                        return new FixedWindowRateLimiterOptions()
                        {
                            AutoReplenishment = true,
                            PermitLimit = 5,
                            QueueLimit = 0,
                            Window = TimeSpan.FromMinutes(10)
                        };
                    });
                });

                //options.RejectionStatusCode = 429;

                options.OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.StatusCode = 429;

                    if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                    {
                        await context.HttpContext.Response.WriteAsync($"Too many requests. Please try again after {retryAfter.TotalSeconds} seconds.", token);
                    }
                    else
                    {
                        await context.HttpContext.Response.WriteAsync($"Too many requests. Please try again later.", token);
                    }
                };
            });

            return services;
        }
    
        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;

            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            return services;
        }

        public static IServiceCollection AddJWTAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            var jwtSettings = new JwtSettings();
            configuration.Bind(JwtSettings.Section, jwtSettings);

            var secretKey = Environment.GetEnvironmentVariable(EnviromentVariableConstant.SECRET);

            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentNullException($"{nameof(AddJWTAuthentication)} Secret Key does not exist.");
            }


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Issuer,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });


            services.AddTransient<IJwtService, JwtService>();
            return services;
        }
    }
}