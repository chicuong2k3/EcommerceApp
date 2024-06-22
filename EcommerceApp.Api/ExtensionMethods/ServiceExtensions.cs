﻿using Asp.Versioning;
using EcommerceApp.DAL.Repositories;
using EcommerceApp.Domain.Interfaces;
using System.Threading.RateLimiting;

namespace EcommerceApp.Api.ExtensionMethods
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
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
                    return RateLimitPartition.GetFixedWindowLimiter("GlobalLimiter", partition =>
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
    }
}