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

    }
}