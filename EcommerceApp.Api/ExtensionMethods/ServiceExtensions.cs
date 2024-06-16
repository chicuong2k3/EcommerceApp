using EcommerceApp.Api.Services.Implementations;
using EcommerceApp.Api.Services.Interfaces;
using EcommerceApp.DAL.Repositories;
using EcommerceApp.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
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

        public static IServiceCollection AddCustomMediaTypes(this IServiceCollection services)
        {

            services.Configure<MvcOptions>(config =>
            {
                var jsonOutputFormatter = config.OutputFormatters.OfType<SystemTextJsonOutputFormatter>()?.FirstOrDefault();

                if (jsonOutputFormatter != null)
                {
                    jsonOutputFormatter.SupportedMediaTypes.Add("application/hal+json");
                }

                //var xmlOutputFormatter = config.OutputFormatters
                //.OfType<XmlDataContractSerializerOutputFormatter>()
                //?.FirstOrDefault();

                //if (xmlOutputFormatter != null)
                //{
                //    xmlOutputFormatter.SupportedMediaTypes.Add("application/hal+xml");
                //}
            });

            return services;
        }
    }
}