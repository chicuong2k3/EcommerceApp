using Asp.Versioning;
using EcommerceApp.DAL.Repositories;
using EcommerceApp.Domain.Interfaces;

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
            return services;
        }
    }
}