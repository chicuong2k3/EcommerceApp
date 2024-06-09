using EcommerceApp.Api.Services.Implementations;
using EcommerceApp.Api.Services.Interfaces;
using EcommerceApp.DAL.Repositories;
using EcommerceApp.Domain.Interfaces;
using NLog;

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

        public static IServiceCollection AddLogger(this IServiceCollection services)
        {
            LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            services.AddSingleton<ILoggerService, LoggerService>();
            return services;
        }
    }
}